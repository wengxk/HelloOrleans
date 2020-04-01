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

        public async Task Trans(GoodsInventoryTransaction trans)
        {
            RaiseEvent(new GoodsInventoryTransactionEvent
            {
                TransactionType = trans.TransactionType,
                Amount = trans.Amount
            });
            await ConfirmEvents();
        }

        public async Task<IEnumerable<GoodsInventoryTransaction>> GetAllTransHist()
        {
            var e = await RetrieveConfirmedEvents(0, Version);

            var result = e.Select(x =>
                new GoodsInventoryTransaction
                {
                    Id = (int) this.GetPrimaryKeyLong(),
                    TransactionType = x.TransactionType,
                    Amount = x.Amount
                }).ToList(); // must call ToList()

            return await Task.FromResult(result.AsEnumerable());
        }

        #endregion
    }


    public class GoodsInventoryTransactionEvent
    {
        public string TransactionType { get; set; }

        public uint Amount { get; set; }
    }


    [Serializable]
    public class GoodsInventoryState
    {
        public uint Inventory { get; set; }

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