namespace HelloOrleans.DomainModels
{
    using System;
    using Events;

    [Serializable]
    public class GoodsInventory
    {
        public long Id { get; set; }

        public uint Inventory { get; set; }

        public GoodsInventory Apply(GoodsInventoryTransactionEvent @event)
        {
            switch (@event.TransactionType)
            {
                case "in":
                    Inventory += @event.Amount;
                    break;
                case "out":
                    Inventory -= @event.Amount;
                    break;
            }

            return this;
        }
    }
}