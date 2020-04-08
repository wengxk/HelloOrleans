namespace HelloOrleans.Grains
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Orleans;

    public class TimerSampleGrain : Grain, ITimerSample
    {
        // refer: https://dotnet.github.io/orleans/Documentation/grains/timers_and_reminders.html
        // 1. A timer will cease to trigger if the activation is deactivated or when a fault occurs and its silo crashes.
        // 2. When activation collection is enabled, the execution of a timer callback does not change the activation's state from idle to in use.
        //    This means that a timer cannot be used to postpone deactivation of otherwise idle activations.    
        //    timer的运行不会该变silo的状态。
        // 3. 上一个 timer 触发的任务如果没有执行完成，则会阻塞下一次 timer 的触发。
        //    即 timer 内的任务运行是线性的，永远不会重复进入，这是和 System.Threading.Timer 的一个重要区别。 
        
        
        private readonly ILogger<TimerSampleGrain> _logger;
        
        public TimerSampleGrain(ILogger<TimerSampleGrain> logger)
        {
            _logger = logger;
        }
        
        private Task TestTimer(object obj)
        {
             Task.Delay(TimeSpan.FromSeconds(9)).Wait(); //加了这个9s，结果就是每12s输出一次。
            _logger.LogInformation(
                $"=========={DateTime.Now.ToLocalTime()}==========\n=========={new Random().Next(100, 1000).ToString()}==========");
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            base.RegisterTimer(TestTimer, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));
            return base.OnActivateAsync();
        }

        public Task Initialize()
        {
            _logger.LogInformation("TimerSampleGrain Initialized");
            return Task.CompletedTask;
        }
    }
}