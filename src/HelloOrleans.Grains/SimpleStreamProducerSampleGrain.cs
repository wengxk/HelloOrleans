namespace HelloOrleans.Grains
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;

    public class SimpleStreamProducerSampleGrain : Grain, ISimpleStreamProducerSample
    {
        private readonly ILogger<SimpleStreamProducerSampleGrain> _logger;
        public SimpleStreamProducerSampleGrain(ILogger<SimpleStreamProducerSampleGrain> logger)
        {
            _logger = logger;
        }
        
        public Task Initialize()
        {
            _logger.LogInformation($"SimpleStreamProducerSampleGrain Initialize: start");
            // get stream provider
            var streamProvider = base.GetStreamProvider("SMSProvider");
            // get the reference to a stream with guid and namespace
            var stream = streamProvider.GetStream<int>(this.GetPrimaryKey(), "SimpleStreamProducerSample");
            // produce stream
            base.RegisterTimer(s => stream.OnNextAsync(new System.Random().Next()), null,
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            _logger.LogInformation($"SimpleStreamProducerSampleGrain Initialize: end");
            return Task.CompletedTask;
        }
    }
}