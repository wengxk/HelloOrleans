namespace HelloOrleans.SiloHost
{
    using System;
    using System.Net;
    using HelloOrleans.Grains;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Configuration;
    using Orleans.Hosting;


    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        #region Methods

        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        public static void Main(string[] args)
        {
            var builder = new SiloHostBuilder()
               .ConfigureApplicationParts(_ => _.AddApplicationPart(typeof(ShoppingCartGarin).Assembly).WithReferences())
               .UseLocalhostClustering()
               .Configure<EndpointOptions>(_ =>
               {
                   _.AdvertisedIPAddress = IPAddress.Loopback;
                   _.SiloPort = 11111;
                   _.GatewayPort = 30000;
               })
               //.AddMemoryGrainStorageAsDefault()
               .AddAdoNetGrainStorage("HelloOrleansStorage", _ =>
               {
                   _.Invariant = "MySql.Data.MySqlClient";
                   _.ConnectionString = "Server=localhost;Database=helloorleans;Uid=root;Pwd=111111";
                   _.UseJsonFormat = true;
               })
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

        #endregion
    }
}
