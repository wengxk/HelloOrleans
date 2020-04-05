﻿namespace HelloOrleans.BlazorClient.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels.Events;
    using Interfaces;
    using Microsoft.Extensions.Logging;
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

        public async Task Trans(GoodsInventoryTransactionEvent goodsInventoryTransaction)
        {
            if (goodsInventoryTransaction == null)
                return;
            await _client.GetGrain<IGoodsInventory>(goodsInventoryTransaction.GoodsId)
                .Trans(goodsInventoryTransaction);
        }

        public async Task<IEnumerable<GoodsInventoryTransactionEvent>> All(long id)
        {
            return await _client.GetGrain<IGoodsInventory>(id).GetAllTransHist();
        }

        public async Task<uint> GetCurrentInventory(long id)
        {
            return await _client.GetGrain<IGoodsInventory>(id).GetCurrentInventory();
        }
    }
}