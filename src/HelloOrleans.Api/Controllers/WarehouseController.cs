namespace HelloOrleans.Api.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels;
    using Interfaces;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public WarehouseController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpGet]
        public async Task<IEnumerable<BasicGoods>> All()
        {
            return await _clusterClient.GetGrain<IWarehouse>(1).All();
        }

        [HttpPost]
        public async Task Add(BasicGoods basicGoods)
        {
            await _clusterClient.GetGrain<IWarehouse>(1).Add(basicGoods);
        }
    }
}