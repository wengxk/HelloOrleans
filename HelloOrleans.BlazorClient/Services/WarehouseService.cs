namespace HelloOrleans.BlazorClient.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using Orleans;

    public class WarehouseService
    {
        private readonly IClusterClient _clusterClient;

        public WarehouseService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public Task<IEnumerable<BasicGoods>> All()
        {
            return _clusterClient.GetGrain<IWarehouse>(1).All();
        }

        public Task Add(BasicGoods basicGoods)
        {
            return _clusterClient.GetGrain<IWarehouse>(1).Add(basicGoods);
        }
    }
}