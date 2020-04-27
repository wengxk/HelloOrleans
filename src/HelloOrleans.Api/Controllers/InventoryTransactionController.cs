namespace HelloOrleans.Api.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels.Events;
    using Interfaces;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Orleans;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InventoryTransactionController : ControllerBase
    {
        private readonly IClusterClient _client;
        private readonly ILogger<InventoryTransactionController> _logger;

        public InventoryTransactionController(IClusterClient client, ILogger<InventoryTransactionController> logger)
        {
            _client = client;
            _logger = logger;
        }

        [HttpPost]
        public async Task Trans(GoodsInventoryTransactionEvent goodsInventoryTransaction)
        {
            if (goodsInventoryTransaction == null)
                return;
            await _client.GetGrain<IGoodsInventory>(goodsInventoryTransaction.GoodsId)
                .Trans(goodsInventoryTransaction);
        }

        [HttpGet]
        public async Task<IEnumerable<GoodsInventoryTransactionEvent>> All(long id)
        {
            return await _client.GetGrain<IGoodsInventory>(id).GetAllTransHist();
        }

        [HttpGet]
        public async Task<uint> GetCurrentInventory(long id)
        {
            return await _client.GetGrain<IGoodsInventory>(id).GetCurrentInventory();
        }
    }
}