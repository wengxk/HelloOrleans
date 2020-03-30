using System.Collections.Generic;
using System.Threading.Tasks;
using HelloOrleans.Interfaces;
using HelloOrleans.Models;
using Orleans;
using Orleans.Runtime;
using System.Linq;

namespace HelloOrleans.Grains
{
    public class WarehouseGrain : Grain, IWarehouse
    {
        private readonly IPersistentState<List<BasicGoods>> _allBasicGoods;

        public WarehouseGrain([PersistentState("allBasicGoods", "HelloOrleansStorage")] IPersistentState<List<BasicGoods>> allGoods)
        {
            _allBasicGoods = allGoods;
        }

        Task<IEnumerable<BasicGoods>> IWarehouse.All()
        {
            return Task.FromResult(_allBasicGoods.State.AsEnumerable());
        }

        public Task Add(BasicGoods basicGoods)
        {
            _allBasicGoods.State.Add(basicGoods);
            _allBasicGoods.WriteStateAsync();
            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            _allBasicGoods.WriteStateAsync();
            return base.OnDeactivateAsync();
        }
    }
}
