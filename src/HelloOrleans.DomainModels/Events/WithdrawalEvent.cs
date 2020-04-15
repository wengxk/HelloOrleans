namespace HelloOrleans.DomainModels.Events
{
    using System;

    [Serializable]
    public class WithdrawalEvent : AccountEvent
    {
        public WithdrawalEvent(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}