namespace HelloOrleans.Api.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Orleans;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(ILogger<ShoppingCartController> logger, IClusterClient clusterClient)
        {
            _logger = logger;
            _clusterClient = clusterClient;
        }

        [HttpPost]
        public async Task Add(string goods)
        {
            await _clusterClient.GetGrain<IShoppingCart>(1).Add(goods);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> All()
        {
            return await _clusterClient.GetGrain<IShoppingCart>(1).All();
        }

        [HttpPost]
        public async Task Clear()
        {
            await _clusterClient.GetGrain<IShoppingCart>(1).Clear();
        }
    }
}