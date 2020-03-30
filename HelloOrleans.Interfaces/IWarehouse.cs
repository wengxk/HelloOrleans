using System.Collections.Generic;
using System.Threading.Tasks;
using HelloOrleans.Models;
using Orleans;

namespace HelloOrleans.Interfaces
{
    public interface IWarehouse : IGrainWithIntegerKey
    {
        Task<IEnumerable<BasicGoods>> All();

        Task Add(BasicGoods basicGoods);
    }
}
