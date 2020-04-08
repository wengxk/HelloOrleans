namespace HelloOrleans.Grains
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;
    using Orleans.Runtime;

    public class ReminderSampleGrain: Grain, IReminderSample
    {
        // refer: https://dotnet.github.io/orleans/Documentation/grains/timers_and_reminders.html#reminders
        // 1. reminder 会被持久化，而 timer 不会被持久化。
        // 2. grain deactivated 之后，在 reminder 下次触发时会重新被激活。
        // 3. reminder 主要用于分钟频率以上的定时任务，即粒度比 timer 要大。
        private readonly ILogger<ReminderSampleGrain> _logger;
        
        public ReminderSampleGrain(ILogger<ReminderSampleGrain> logger)
        {
            _logger = logger;
        }

        public Task ReceiveReminder(string reminderName, TickStatus status)
        {
            _logger.LogInformation("===========================================================================");
            _logger.LogInformation($"Reminder {reminderName} received at {DateTime.Now.ToLocalTime()}!");
            _logger.LogInformation($" status.FirstTickTime is {status.FirstTickTime.ToLocalTime()} \n " +
                                   $"status.CurrentTickTime is {status.CurrentTickTime.ToLocalTime()} \n " +
                                   $"status.Period is {status.Period.TotalMinutes} in minutes");
            return Task.CompletedTask;
        }

        public Task Initialize()
        {
            _logger.LogInformation("ReminderSampleGrain Initialized");
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            base.RegisterOrUpdateReminder("ReminderSample", TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
            return base.OnActivateAsync();
        }
    }
}