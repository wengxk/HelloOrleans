namespace HelloOrleans.Grains
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using Orleans;
    using Orleans.EventSourcing;
    using Orleans.Providers;

    [StorageProvider(ProviderName = "HelloOrleansStorage")]
    [LogConsistencyProvider(ProviderName = "LogStorage")]
    public class GoodsInventoryTransactionGrain : JournaledGrain<GoodsInventoryState, GoodsInventoryTransactionEvent>,
        IGoodsInventoryTransaction
    {
        #region IGoodsInventoryTransaction Members

        // public Task StockIn(uint amount)
        // {
        //     RaiseEvent(new StockInEvent(amount));
        //     return ConfirmEvents();
        // }
        //
        // public Task StockOut(uint amount)
        // {
        //     RaiseEvent(new StockOutEvent(amount));
        //     return ConfirmEvents();
        // }


        public Task Trans(GoodsInventoryTransaction trans)
        {
            RaiseEvent(new GoodsInventoryTransactionEvent
            {
                TransactionType = trans.TransactionType,
                Amount = trans.Amount
            });
            return ConfirmEvents();
        }

        public Task<IEnumerable<GoodsInventoryTransaction>> GetAllTransHist()
        {
            var e = RetrieveConfirmedEvents(0, Version);
            e.Wait();

            var result = e.Result.Select(x =>
                new GoodsInventoryTransaction
                {
                    Id = (int) this.GetPrimaryKeyLong(),
                    TransactionType = x.TransactionType,
                    Amount = x.Amount
                }).ToList(); // must call ToList()

            return Task.FromResult(result.AsEnumerable());
        }

        #endregion
    }


    public class GoodsInventoryTransactionEvent
    {
        public string TransactionType { get; set; }

        public uint Amount { get; set; }
    }

    // public class StockInEvent : GoodsInventoryEvent
    // {
    //     public StockInEvent(uint amount)
    //     {
    //         Amount = amount;
    //     }
    //
    //     public uint Amount { get; }
    // }
    //
    //
    // public class StockOutEvent : GoodsInventoryEvent
    // {
    //     public StockOutEvent(uint amount)
    //     {
    //         Amount = amount;
    //     }
    //
    //     public uint Amount { get; }
    // }

    [Serializable]
    public class GoodsInventoryState
    {
        public uint Inventory { get; set; }

        // public GoodsInventoryState Apply(StockInEvent @event)
        // {
        //     this.Inventory += @event.Amount;
        //     return this;
        // }
        //
        // public GoodsInventoryState Apply(StockOutEvent @event)
        // {
        //     this.Inventory -= @event.Amount;
        //     return this;
        // }

        public GoodsInventoryState Apply(GoodsInventoryTransactionEvent @event)
        {
            if (@event.TransactionType.Equals("in"))
                Inventory += @event.Amount;
            else
                Inventory -= @event.Amount;

            return this;
        }
    }
}