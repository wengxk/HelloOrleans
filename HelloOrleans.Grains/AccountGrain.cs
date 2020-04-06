namespace HelloOrleans.Grains
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using DomainModels.Events;
    using Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Models;
    using NodaTime;
    using Npgsql;
    using NpgsqlTypes;
    using Orleans;
    using Orleans.EventSourcing;

    //refer: https://dotnet.github.io/orleans/Documentation/grains/event_sourcing/log_consistency_providers.html
    public class AccountGrain : JournaledGrain<Account, AccountEvent>, IAccount
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountGrain> _logger;

        public AccountGrain(ILogger<AccountGrain> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        #region IAccount Members

        public async Task Deposit(decimal amount)
        {
            RaiseEvent(new DepositEvent(amount)
            {
                AccountId = this.GetPrimaryKeyLong()
            });
            await ConfirmEvents();
        }

        public async Task Withdrawal(decimal amount)
        {
            RaiseEvent(new WithdrawalEvent(amount)
            {
                AccountId = this.GetPrimaryKeyLong()
            });
            await ConfirmEvents();
        }

        public async Task<IEnumerable<AccountEvent>> RetrieveConfirmedEvents()
        {
            var result = new List<AccountEvent>();
            await using var conn = new NpgsqlConnection(Common.ConnectionString);
            await conn.OpenAsync();
            conn.TypeMapper.UseNodaTime();

            await using var cmd = new NpgsqlCommand(
                "select account_id ,etag ,\"timestamp\" ,transaction_type ,amount from account_events where account_id = @id order by etag desc"
                , conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Bigint, this.GetPrimaryKeyLong());

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows) return result;

            while (await reader.ReadAsync())
            {
                var et = reader.GetString(3);
                switch (et)
                {
                    case "Deposit":
                        var de = new DepositEvent(reader.GetDecimal(4))
                        {
                            AccountId = this.GetPrimaryKeyLong(),
                            ETag = reader.GetInt32(1),
                            Timestamp = reader.GetFieldValue<Instant>(2).ToDateTimeOffset()
                        };
                        result.Add(de);
                        break;
                    case "Withdrawal":
                        var we = new WithdrawalEvent(reader.GetDecimal(4))
                        {
                            AccountId = this.GetPrimaryKeyLong(),
                            ETag = reader.GetInt32(1),
                            Timestamp = reader.GetFieldValue<Instant>(2).ToDateTimeOffset()
                        };
                        result.Add(we);
                        break;
                }
            }

            await conn.CloseAsync();
            return result;
        }

        public Task<decimal> GetBalance()
        {
            return Task.FromResult(State.Balance);
        }

        // The first method, ReadStateFromStorage, is expected to return both the version, and the state read.
        // If there is nothing stored yet, it should return zero for the version and a state that matches corresponds to the default constructor for StateType.
        public async Task<KeyValuePair<int, Account>> ReadStateFromStorage()
        {
            _logger.LogInformation("ReadStateFromStorage: start");
            await using var conn = new NpgsqlConnection(Common.ConnectionString);
            await conn.OpenAsync();
            conn.TypeMapper.UseNodaTime();

            var state = await HandleReadSnapshot(conn);
            if (state.Etag == -1)
            {
                state.Etag = 0;
                await InitSnapshot(conn, state);
            }
            var etag = state.Etag;

            await GetNewState(conn, state);
            var newEtag = state.Etag;
            if (newEtag > etag)
                await PersistState(conn, state);
            await conn.CloseAsync();

            _logger.LogInformation("ReadStateFromStorage: end");
            return new KeyValuePair<int, Account>(state.Etag, state);
        }

        // ApplyUpdatesToStorage must return false if the expected version does not match the actual version (this is analogous to an e-tag check).
        public async Task<bool> ApplyUpdatesToStorage(IReadOnlyList<AccountEvent> updates, int expectedVersion)
        {
            _logger.LogInformation($"ApplyUpdatesToStorage: start with expected version {expectedVersion}");
            await using var conn = new NpgsqlConnection(Common.ConnectionString);
            await conn.OpenAsync();
            conn.TypeMapper.UseNodaTime();

            var actualVersion = await GetActualVersion(conn);
            _logger.LogInformation($"get actual version is {actualVersion}");
            if (expectedVersion != actualVersion) return false;

            foreach (var e in updates)
            {
                actualVersion++;
                if (e.ETag != AccountEvent.NEW_ETag) continue;
                e.ETag = actualVersion;
                await PersistEvent(conn, e);
            }

            await conn.CloseAsync();

            _logger.LogInformation($"ApplyUpdatesToStorage: end with version {actualVersion}");
            return true;
        }

        #endregion


        private async Task PersistEvent(NpgsqlConnection conn, AccountEvent e)
        {
            if (!(e is DepositEvent) && !(e is WithdrawalEvent)) return;
            _logger.LogInformation("handle new event: start");
            await using var cmd = new NpgsqlCommand(
                "insert into account_events (account_id ,etag ,\"timestamp\" ,transaction_type ,amount )values(@id,@etag,@tm,@ttype,@amount)"
                , conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Bigint, e.AccountId);
            cmd.Parameters.AddWithValue("etag", NpgsqlDbType.Integer, e.ETag);
            cmd.Parameters.Add(new NpgsqlParameter("tm", Instant.FromDateTimeOffset(e.Timestamp)));
            switch (e)
            {
                case DepositEvent @event:
                    cmd.Parameters.AddWithValue("ttype", NpgsqlDbType.Varchar, "Deposit");
                    cmd.Parameters.AddWithValue("amount", NpgsqlDbType.Money, @event.Amount);
                    break;
                case WithdrawalEvent @event:
                    cmd.Parameters.AddWithValue("ttype", NpgsqlDbType.Varchar, "Withdrawal");
                    cmd.Parameters.AddWithValue("amount", NpgsqlDbType.Money, @event.Amount);
                    break;
            }

            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("handle new event: end");
        }

        private async Task<int> GetActualVersion(NpgsqlConnection conn)
        {
            await using var cmd = new NpgsqlCommand(
                "select COALESCE(max(etag),0) from account_events where account_id  = @id"
                , conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Bigint, this.GetPrimaryKeyLong());
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows) return 0;
            await reader.ReadAsync();
            return reader.GetInt32(0);
        }


        private async Task<Account> HandleReadSnapshot(NpgsqlConnection conn)
        {
            var state = new Account
            {
                Id = this.GetPrimaryKeyLong(),
                Timestamp = DateTimeOffset.Now,
                Etag = -1, //无快照数据
                Balance = 0
            };

            await using var cmd = new NpgsqlCommand(
                "select t.account_id, t.etag , t.\"timestamp\" , t.balance from account_snapshots t where account_id = @id"
                , conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Bigint, this.GetPrimaryKeyLong());
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows) return state;

            await reader.ReadAsync();
            state.Etag = reader.GetInt32(1);
            state.Timestamp = reader.GetFieldValue<Instant>(2).ToDateTimeOffset();
            state.Balance = reader.GetDecimal(3);

            return state;
        }

        private async Task InitSnapshot(NpgsqlConnection conn, Account state)
        {
            _logger.LogInformation("insert snapshot: start");
            await using var cmd = new NpgsqlCommand(
                "insert into account_snapshots(account_id,etag ,\"timestamp\" ,balance ) values(@id,@etag,@tm,@balance)"
                , conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Bigint, state.Id);
            cmd.Parameters.AddWithValue("etag", NpgsqlDbType.Integer, state.Etag);
            cmd.Parameters.Add(new NpgsqlParameter("tm", Instant.FromDateTimeOffset(state.Timestamp)));
            cmd.Parameters.AddWithValue("balance", NpgsqlDbType.Money, state.Balance);
            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("insert snapshot: end");
        }


        private async Task GetNewState(NpgsqlConnection conn, Account state)
        {
            await using var cmd = new NpgsqlCommand(
                @"select ae.account_id account_id, sum(case ae.transaction_type  
                when 'Deposit' then ae.amount 
                when 'Withdrawal' then -1*ae.amount end) balance ,
                max(ae.etag ) etag,max(ae.""timestamp"" ) tm from account_events ae where ae.account_id = @id and ae.etag > @etag group by ae.account_id "
                , conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Bigint, state.Id);
            cmd.Parameters.AddWithValue("etag", NpgsqlDbType.Integer, state.Etag);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows) return;
            await reader.ReadAsync();
            state.Etag = reader.GetInt32(2);
            state.Timestamp = reader.GetFieldValue<Instant>(3).ToDateTimeOffset();
            state.Balance = reader.GetDecimal(1);
        }

        private async Task PersistState(NpgsqlConnection conn, Account state)
        {
            _logger.LogInformation("persist state snapshot: start");
            await using var cmd = new NpgsqlCommand(
                "update account_snapshots set etag = @etag, \"timestamp\" = @tm, balance = @balance where account_id  = @id"
                , conn);
            cmd.Parameters.AddWithValue("etag", NpgsqlDbType.Integer, state.Etag);
            cmd.Parameters.Add(new NpgsqlParameter("tm", Instant.FromDateTimeOffset(state.Timestamp)));
            cmd.Parameters.AddWithValue("balance", NpgsqlDbType.Money, state.Balance);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Bigint, state.Id);
            await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("persist state snapshot: end");
        }
    }
}