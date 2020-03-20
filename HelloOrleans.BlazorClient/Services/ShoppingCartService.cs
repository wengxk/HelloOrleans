using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using HelloOrleans.Interfaces;

namespace HelloOrleans.BlazorClient.Services
{
    public class ShoppingCartService
    {

        private readonly ILogger<ShoppingCartService> _logger;
        private readonly IClusterClient _clusterClient;

        public ShoppingCartService(ILogger<ShoppingCartService> logger,IClusterClient clusterClient)
        {
            _logger = logger;
            _clusterClient = clusterClient;
        }

        public Task Add(string goods)
        {
          return  _clusterClient.GetGrain<IShoppingCart>(0).Add(goods);
        }


        public Task<IEnumerable<string>> All()
        {
            return _clusterClient.GetGrain<IShoppingCart>(0).All();
        }

        public Task Clear()
        {
            return _clusterClient.GetGrain<IShoppingCart>(0).Clear();
        }
    }
}
