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
    public class BankAccountController : ControllerBase
    {
        private readonly IClusterClient _client;
        private readonly ILogger<BankAccountController> _logger;

        public BankAccountController(IClusterClient client, ILogger<BankAccountController> logger)
        {
            _client = client;
            _logger = logger;
        }

        [HttpPost]
        public async Task Deposit(decimal amount)
        {
            await _client.GetGrain<IAccount>(1).Deposit(amount);
        }

        [HttpPost]
        public async Task Withdrawal(decimal amount)
        {
            await _client.GetGrain<IAccount>(1).Withdrawal(amount);
        }

        [HttpGet]
        public async Task<IEnumerable<AccountEvent>> GetAllHists()
        {
            return await _client.GetGrain<IAccount>(1).RetrieveConfirmedEvents();
        }

        [HttpGet]
        public async Task<decimal> GetBalance()
        {
            return await _client.GetGrain<IAccount>(1).GetBalance();
        }
    }
}