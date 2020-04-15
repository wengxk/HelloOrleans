namespace HelloOrleans.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels;
    using Orleans;

    public interface IWarehouse : IGrainWithIntegerKey
    {
        Task<IEnumerable<BasicGoods>> All();

        Task Add(BasicGoods basicGoods);
    }
}