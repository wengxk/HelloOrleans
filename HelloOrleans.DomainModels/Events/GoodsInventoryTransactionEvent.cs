namespace HelloOrleans.DomainModels.Events
{
    public class GoodsInventoryTransactionEvent
    {
        public string TransactionType { get; set; }

        public uint Amount { get; set; }
    }
}