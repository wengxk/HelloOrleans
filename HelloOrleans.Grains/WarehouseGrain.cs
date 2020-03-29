using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloOrleans.Interfaces;
using HelloOrleans.Models;
using Orleans;

namespace HelloOrleans.Grains
{
    class WarehouseGrain : Grain, IWarehouse
    {
        public Task<IEnumerable<GoodsInventory>> All()
        {
            var result = new GoodsInventory[] { new GoodsInventory(){
                Id = 1,
                Goods = "milk",
                Inventory = 100
            } };

            return Task.FromResult(result.AsEnumerable());
        }
    }
}
