namespace HelloOrleans.BlazorClient.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;
    using Orleans;

    public class InventoryTransactionService
    {
        private readonly IClusterClient _client;
        private readonly ILogger<InventoryTransactionService> _logger;

        public InventoryTransactionService(IClusterClient client, ILogger<InventoryTransactionService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task Trans(GoodsInventoryTransaction goodsInventoryTransaction)
        {
            if (goodsInventoryTransaction == null)
                return;
            await _client.GetGrain<IGoodsInventoryTransaction>(goodsInventoryTransaction.Id)
                .Trans(goodsInventoryTransaction);
        }

        public async Task<IEnumerable<GoodsInventoryTransaction>> All(int id)
        {
            return await _client.GetGrain<IGoodsInventoryTransaction>(id).GetAllTransHist();
        }

        public async Task<uint> GetCurrentInventory(int id)
        {
            return await _client.GetGrain<IGoodsInventoryTransaction>(id).GetCurrentInventory();
        }
    }
}