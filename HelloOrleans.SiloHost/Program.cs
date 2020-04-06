namespace HelloOrleans.SiloHost
{
    using System;
    using System.Net;
    using Grains;
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
            HelloOrleans.Grains.Common.ConnectionString = connectionString;
            
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
                .AddLogStorageBasedLogConsistencyProvider("LogStorage")
                .AddCustomStorageBasedLogConsistencyProviderAsDefault("CustomLogStorage")
                .ConfigureLogging(builder =>
                    builder
                        .AddConsole())
                .UseDashboard(options =>
                {
                    options.Port = 8000;
                    options.HideTrace = true;
                });

            using var host = builder.Build();
            host.StartAsync().Wait();
            Console.WriteLine("\n\n Press Enter to terminate...\n\n");
            Console.ReadLine();
            host.StopAsync().Wait();
        }
    }
}