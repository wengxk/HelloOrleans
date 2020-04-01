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

        public async Task<IEnumerable<BasicGoods>> All()
        {
            return await Task.FromResult(_allBasicGoods.State.AsEnumerable());
        }

        public async Task Add(BasicGoods basicGoods)
        {
            _allBasicGoods.State.Add(basicGoods);
            await _allBasicGoods.WriteStateAsync();
        }

        #endregion

        public override async Task OnDeactivateAsync()
        {
            await _allBasicGoods.WriteStateAsync();
        }
    }
}