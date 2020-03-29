using System.Collections.Generic;
using System.Threading.Tasks;
using HelloOrleans.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;

namespace HelloOrleans.BlazorClient.Services
{
    public class ShoppingCartService
    {

        private readonly ILogger<ShoppingCartService> _logger;
        private readonly IClusterClient _clusterClient;

        public ShoppingCartService(ILogger<ShoppingCartService> logger, IClusterClient clusterClient)
        {
            _logger = logger;
            _clusterClient = clusterClient;
        }

        public Task Add(string goods)
        {
            return _clusterClient.GetGrain<IShoppingCart>(1).Add(goods);
        }


        public Task<IEnumerable<string>> All()
        {
            return _clusterClient.GetGrain<IShoppingCart>(1).All();
        }

        public Task Clear()
        {
            return _clusterClient.GetGrain<IShoppingCart>(1).Clear();
        }
    }
}
