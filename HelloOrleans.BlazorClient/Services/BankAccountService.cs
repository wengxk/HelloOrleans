namespace HelloOrleans.BlazorClient.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels.Events;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;

    public class BankAccountService
    {
        private readonly IClusterClient _client;
        private readonly ILogger<BankAccountService> _logger;

        public BankAccountService(IClusterClient client, ILogger<BankAccountService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task Deposit(decimal amount)
        {
            await _client.GetGrain<IAccount>(1).Deposit(amount);
        }

        public async Task Withdrawal(decimal amount)
        {
            await _client.GetGrain<IAccount>(1).Withdrawal(amount);
        }


        public async Task<IEnumerable<AccountEvent>> GetAllHists()
        {
            return await _client.GetGrain<IAccount>(1).RetrieveConfirmedEvents();
        }

        public async Task<decimal> GetBalance()
        {
            return await _client.GetGrain<IAccount>(1).GetBalance();
        }
    }
}