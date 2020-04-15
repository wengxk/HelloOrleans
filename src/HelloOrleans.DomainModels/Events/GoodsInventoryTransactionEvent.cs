namespace HelloOrleans.DomainModels.Events
{
    using System;

    [Serializable]
    public class GoodsInventoryTransactionEvent
    {
        public long GoodsId { get; set; }
        
        public string TransactionType { get; set; }

        public uint Amount { get; set; }
    }
}