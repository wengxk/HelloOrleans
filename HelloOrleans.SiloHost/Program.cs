namespace HelloOrleans.SiloHost
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Grains;
    using Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Configuration;
    using Orleans.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();
            var connectionString = configuration.GetConnectionString("aliyunpgsql");
            Common.ConnectionString = connectionString;

            var builder = new SiloHostBuilder()
                .ConfigureApplicationParts(builder =>
                    builder.AddApplicationPart(typeof(ShoppingCartGarin).Assembly).WithReferences())
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = IPAddress.Loopback;
                    options.SiloPort = 11111;
                    options.GatewayPort = 30000;
                })
                .AddAdoNetGrainStorage("HelloOrleansStorage", options =>
                {
                    options.Invariant = "Npgsql";
                    options.ConnectionString = connectionString;
                    options.UseJsonFormat = true;
                })
                .AddLogStorageBasedLogConsistencyProvider()
                .AddCustomStorageBasedLogConsistencyProviderAsDefault("CustomLogStorage")
                .ConfigureLogging(builder =>
                    builder
                        .AddConsole())
                .AddSimpleMessageStreamProvider("SMSProvider")
                .AddMemoryGrainStorage("PubSubStore")
                // .AddAdoNetGrainStorage("PubSubStore", options =>
                // {
                //     options.Invariant = "Npgsql";
                //     options.ConnectionString = connectionString;
                //     options.UseJsonFormat = true;
                // })
                .UseDashboard(options =>
                {
                    options.Port = 8000;
                    options.HideTrace = true;
                })
                .AddStartupTask(ConfigureStartupTasks)
                .UseAdoNetReminderService(options =>
                {
                    options.Invariant = "Npgsql";
                    options.ConnectionString = connectionString;
                })
                ;

            using var host = builder.Build();
            host.StartAsync().Wait();
            Console.WriteLine("\n\n Press Enter to terminate...\n\n");
            Console.ReadLine();
            host.StopAsync().Wait();
        }


        // Pre-loading some grains
        private static async Task ConfigureStartupTasks(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            // Use the service provider to get the grain factory.
            var grainFactory = serviceProvider.GetRequiredService<IGrainFactory>();
                        
            // Get a reference to a grain and call a method on it.
            var timerSampleGrain = grainFactory.GetGrain<ITimerSample>(0);
            var reminderSampleGrain = grainFactory.GetGrain<IReminderSample>(0);
            
            // stream sample
            var guid = new System.Guid();
            var simpleStreamProducerGrain = grainFactory.GetGrain<ISimpleStreamProducerSample>(guid);
            var guid2 = new System.Guid();
            var simpleStreamConsumerGrain = grainFactory.GetGrain<ISimpleStreamConsumerSample>(guid2);

            await Task.WhenAll(timerSampleGrain.Initialize(), reminderSampleGrain.Initialize(),
                simpleStreamProducerGrain.Initialize(), simpleStreamConsumerGrain.Initialize());
        }
        
    }
}