namespace HelloOrleans.Grains
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using Orleans;
    using Orleans.Runtime;

    public class WarehouseGrain : Grain, IWarehouse
    {
        private readonly IPersistentState<List<BasicGoods>> _allBasicGoods;

        public WarehouseGrain([PersistentState("allBasicGoods", "HelloOrleansStorage")]
            IPersistentState<List<BasicGoods>> allGoods)
        {
            _allBasicGoods = allGoods;
        }

        #region IWarehouse Members

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

        #endregion

        public override Task OnDeactivateAsync()
        {
            _allBasicGoods.WriteStateAsync();
            return base.OnDeactivateAsync();
        }
    }
}