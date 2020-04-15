namespace HelloOrleans.Grains
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Streams;

    [ImplicitStreamSubscription("SimpleStreamProducerSample")]
    public class SimpleStreamConsumerSampleGrain : Grain, ISimpleStreamConsumerSample
    {
        private readonly ILogger<SimpleStreamConsumerSampleGrain> _logger;

        public SimpleStreamConsumerSampleGrain(ILogger<SimpleStreamConsumerSampleGrain> logger)
        {
            _logger = logger;
        }

        public async Task Initialize()
        {
            _logger.LogInformation($"SimpleStreamConsumerSampleGrain Initialize: start");
            var streamProvider = GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<int>(this.GetPrimaryKey(), "SimpleStreamProducerSample");
            var observer = new SampleConsumer();
            await stream.SubscribeAsync(observer);
            _logger.LogInformation($"SimpleStreamConsumerSampleGrain Initialize: end");
        }


        private class SampleConsumer : IAsyncBatchObserver<int>
        {
            public Task OnNextAsync(IList<SequentialItem<int>> items)
            {
                foreach (var item in items)
                {
                    Console.WriteLine($"received data: {item.Item.ToString()}");
                }

                return Task.CompletedTask;
            }

            public Task OnCompletedAsync()
            {
                Console.WriteLine("SampleConsumer completed");
                return Task.CompletedTask;
            }

            public Task OnErrorAsync(Exception ex)
            {
                Console.WriteLine($"SampleConsumer error: {ex.Message}");
                return Task.CompletedTask;
            }
        }
    }
}