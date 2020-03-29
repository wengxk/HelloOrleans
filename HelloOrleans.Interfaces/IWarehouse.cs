using System.Collections.Generic;
using System.Threading.Tasks;
using HelloOrleans.Models;
using Orleans;

namespace HelloOrleans.Interfaces
{
    public interface IWarehouse : IGrainWithIntegerKey
    {
        Task<IEnumerable<GoodsInventory>> All();
    }
}
