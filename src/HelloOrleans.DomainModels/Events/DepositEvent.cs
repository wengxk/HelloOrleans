namespace HelloOrleans.DomainModels.Events
{
    using System;

    [Serializable]
    public class DepositEvent : AccountEvent
    {
        public DepositEvent() { }

        public DepositEvent(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}