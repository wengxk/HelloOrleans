namespace HelloOrleans.Grains
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DomainModels;
    using DomainModels.Events;
    using Interfaces;
    using Orleans;
    using Orleans.EventSourcing;
    using Orleans.Providers;

    [StorageProvider(ProviderName = "HelloOrleansStorage")]
    [LogConsistencyProvider(ProviderName = "LogStorage")]
    public class GoodsInventoryTransactionGrain : JournaledGrain<GoodsInventory, GoodsInventoryTransactionEvent>,
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
                    Id = this.GetPrimaryKeyLong(),
                    TransactionType = x.TransactionType,
                    Amount = x.Amount
                }).ToList(); // must call ToList()

            return await Task.FromResult(result.AsEnumerable());
        }

        public Task<uint> GetCurrentInventory()
        {
            return Task.FromResult(State.Inventory);
        }

        #endregion
    }
}