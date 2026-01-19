using Quartz;
using Scheduler.Tasks;

namespace Scheduler.Modules
{
    public static class SchedulerModule
    {
        public static IServiceCollection RegisterSchedulerTasks(this IServiceCollection services, IConfiguration config)
        {



            // --------------------------------------- Expired cart items: -------------------------------------------------------


            var hourStr = config.GetSection("Config:Local:Scheduler:StartDateTimes:CartItemLock:hour").Value.TrimStart(new Char[] { '0' });
            var hour = 0;
            int.TryParse(hourStr, out hour);
            var minuteStr = config.GetSection("Config:Local:Scheduler:StartDateTimes:CartItemLock:minute").Value.TrimStart(new Char[] { '0' });
            var minute = 0;
            int.TryParse(minuteStr, out minute);

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                var jobKey = new JobKey("LockCartItem");
                q.AddJob<CartItemLocker>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("LockCartItem-trigger")
                    .WithDailyTimeIntervalSchedule(
                            s => s.WithIntervalInHours(24)
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute))

                //.WithDailyTimeIntervalSchedule(s => s.StartingDailyAt(new TimeOfDay(13, 18))));     
                // OR Cron:
                //.WithCronSchedule("0 47 14 ? * * *"));  // "0 0 8 * * ?" --- every day at 8am
                ));
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


            // ---------------------------------------------------------------------------------------------------------------------



            return services;
        }
    }
}
