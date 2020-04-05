namespace HelloOrleans.DomainModels
{
    using System;

    [Serializable]
    public class GoodsInventoryTransaction
    {
        public long Id { get; set; }

        public string TransactionType { get; set; }

        public uint Amount { get; set; }
    }
}