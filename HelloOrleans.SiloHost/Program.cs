namespace HelloOrleans.SiloHost
{
    using System;
    using System.Net;
    using Grains;
    using Microsoft.Extensions.Configuration;
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

            var builder = new SiloHostBuilder()
                .ConfigureApplicationParts(_ =>
                    _.AddApplicationPart(typeof(ShoppingCartGarin).Assembly).WithReferences())
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(_ =>
                {
                    _.AdvertisedIPAddress = IPAddress.Loopback;
                    _.SiloPort = 11111;
                    _.GatewayPort = 30000;
                })
                .AddAdoNetGrainStorage("HelloOrleansStorage", _ =>
                {
                    _.Invariant = "Npgsql";
                    _.ConnectionString = connectionString;
                    _.UseJsonFormat = true;
                })
                .AddLogStorageBasedLogConsistencyProvider("LogStorage")
                .AddStateStorageBasedLogConsistencyProvider("StateLogStorage")
                .ConfigureLogging(_ => _.AddConsole())
                .UseDashboard(_ =>
                {
                    _.Port = 8000;
                    _.HideTrace = true;
                });

            using var host = builder.Build();
            host.StartAsync().Wait();
            Console.WriteLine("\n\n Press Enter to terminate...\n\n");
            Console.ReadLine();
            host.StopAsync().Wait();
        }
    }
}