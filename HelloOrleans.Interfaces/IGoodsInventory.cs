namespace HelloOrleans.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels.Events;
    using Orleans;

    public interface IGoodsInventory : IGrainWithIntegerKey
    {
        Task Trans(GoodsInventoryTransactionEvent transaction);

        Task<IEnumerable<GoodsInventoryTransactionEvent>> GetAllTransHist();

        Task<uint> GetCurrentInventory();
    }
}