﻿namespace HelloOrleans.BlazorClient.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using HelloOrleans.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Configuration;
    using Orleans.Hosting;

    /// <summary>
    /// Defines the <see cref="ClusterService" />
    /// </summary>
    public class ClusterService : IHostedService
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private readonly ILogger<ClusterService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterService"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{ClusterService}"/></param>
        public ClusterService(ILogger<ClusterService> logger)
        {
            this.logger = logger;

            Client = new ClientBuilder()
                .ConfigureApplicationParts(manager => manager.AddApplicationPart(typeof(IShoppingCart).Assembly).WithReferences())
                .UseLocalhostClustering()
                 //.Configure<ClusterOptions>(_ =>
                 //{
                 //    _.ServiceId = "HelloOrleans";
                 //})
                .Build();
        }

        /// <summary>
        /// The StartAsync
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Client.Connect(async error =>
            {
                logger.LogError(error, error.Message);
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                return true;
            });
        }

        /// <summary>
        /// The StopAsync
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public Task StopAsync(CancellationToken cancellationToken) => Client.Close();

        /// <summary>
        /// Gets the Client
        /// </summary>
        public IClusterClient Client { get; }
    }

    /// <summary>
    /// Defines the <see cref="ClusterServiceBuilderExtensions" />
    /// </summary>
    public static class ClusterServiceBuilderExtensions
    {
        /// <summary>
        /// The AddClusterService
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddClusterService(this IServiceCollection services)
        {
            services.AddSingleton<ClusterService>();
            services.AddSingleton<IHostedService>(_ => _.GetService<ClusterService>());
            services.AddTransient(_ => _.GetService<ClusterService>().Client);
            return services;
        }
    }
}