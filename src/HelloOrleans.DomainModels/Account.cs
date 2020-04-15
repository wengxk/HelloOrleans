namespace HelloOrleans.Models
{
    using System;
    using System.Net.Sockets;
    using DomainModels.Events;

    public class Account
    {
        public long Id { get; set; }
        
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        
        public int Etag { get; set; }
        
        public decimal Balance { get; set; }


        public Account Apply(DepositEvent @event)
        {
            Balance += @event.Amount;
            return this;
        }
        
        public Account Apply(WithdrawalEvent @event)
        {
            Balance -= @event.Amount;
            return this;
        }
    }
}