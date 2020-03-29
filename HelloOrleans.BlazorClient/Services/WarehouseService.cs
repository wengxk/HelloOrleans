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

        public Task<IEnumerable<GoodsInventory>> All()
        {
            return _clusterClient.GetGrain<IWarehouse>(1).All();
        }
    }
}
