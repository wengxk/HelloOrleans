using System.Collections.Generic;
using System.Threading.Tasks;
using HelloOrleans.Interfaces;
using HelloOrleans.Models;
using Orleans;

namespace HelloOrleans.BlazorClient.Services
{
    public class WarehouseService
    {
        private readonly IClusterClient _clusterClient;

        public WarehouseService(IClusterClient clusterClient)
        {
            this._clusterClient = clusterClient;
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
