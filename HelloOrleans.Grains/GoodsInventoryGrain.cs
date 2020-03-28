namespace HelloOrleans.Grains
{
    using System;
    using System.Threading.Tasks;
    using HelloOrleans.Interfaces;
    using Orleans.EventSourcing;

    public class GoodsInventoryGrain : JournaledGrain<GoodsInventoryState, GoodsInventoryEvent>, IGoodsInventory
    {
        public Task StockIn(uint amount)
        {
            RaiseEvent(new StockInEvent(amount));
            return ConfirmEvents();
        }

        public Task StockOut(uint amount)
        {
            RaiseEvent(new StockOutEvent(amount));
            return ConfirmEvents();
        }
    }



    public abstract class GoodsInventoryEvent { }

    public class StockInEvent : GoodsInventoryEvent
    {
        public StockInEvent(uint amount)
        {
            Amount = amount;
        }

        public uint Amount { get; }
    }


    public class StockOutEvent : GoodsInventoryEvent
    {
        public StockOutEvent(uint amount)
        {
            Amount = amount;
        }

        public uint Amount { get; }
    }

    [Serializable]
    public class GoodsInventoryState
    {
        public string Goods { get; set; }

        public uint Inventory { get; set; }

        public GoodsInventoryState Apply(StockInEvent @event)
        {
            this.Inventory += @event.Amount;
            return this;
        }

        public GoodsInventoryState Apply(StockOutEvent @event)
        {
            this.Inventory -= @event.Amount;
            return this;
        }
    }


}
