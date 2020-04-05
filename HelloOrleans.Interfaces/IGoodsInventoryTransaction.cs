namespace HelloOrleans.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels;
    using Orleans;

    public interface IGoodsInventoryTransaction : IGrainWithIntegerKey
    {
        Task Trans(GoodsInventoryTransaction transaction);

        Task<IEnumerable<GoodsInventoryTransaction>> GetAllTransHist();

        Task<uint> GetCurrentInventory();
    }
}