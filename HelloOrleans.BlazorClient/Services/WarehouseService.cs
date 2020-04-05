namespace HelloOrleans.BlazorClient.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels;
    using Interfaces;
    using Orleans;

    public class WarehouseService
    {
        private readonly IClusterClient _clusterClient;

        public WarehouseService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public async Task<IEnumerable<BasicGoods>> All()
        {
            return await _clusterClient.GetGrain<IWarehouse>(1).All();
        }

        public async Task Add(BasicGoods basicGoods)
        {
            await _clusterClient.GetGrain<IWarehouse>(1).Add(basicGoods);
        }
    }
}