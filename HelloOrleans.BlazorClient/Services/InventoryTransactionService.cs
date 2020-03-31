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

        public Task Trans(GoodsInventoryTransaction goodsInventoryTransaction)
        {
            if (goodsInventoryTransaction == null)
                return Task.CompletedTask;
            return _client.GetGrain<IGoodsInventoryTransaction>(goodsInventoryTransaction.Id)
                .Trans(goodsInventoryTransaction);
        }

        public Task<IEnumerable<GoodsInventoryTransaction>> All(int id)
        {
            return _client.GetGrain<IGoodsInventoryTransaction>(id).GetAllTransHist();
        }
    }
}