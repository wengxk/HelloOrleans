namespace HelloOrleans.BlazorClient.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;

    public class ShoppingCartService
    {
        private readonly IClusterClient _clusterClient;

        private readonly ILogger<ShoppingCartService> _logger;

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