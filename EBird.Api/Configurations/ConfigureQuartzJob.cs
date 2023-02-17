using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Api.QuartzJob;
using Quartz;

namespace EBird.Api.Configurations
{
    public static class ConfigureQuartzJob
    {
        public static IServiceCollection AddQuartzJob(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                var matchingJob = new JobKey("MatchingJob");

                q.AddJob<MatchingJob>(j => j.WithIdentity(matchingJob));
                q.AddTrigger(t => t
                    .ForJob(matchingJob)
                    .WithIdentity("MatchingJob-Trigger")
                    // .WithCronSchedule("0 0 0 * * ?");
                    .WithSimpleSchedule(x => 
                    {
                        x.WithIntervalInSeconds(1);
                        x.RepeatForever();
                    }));

            });

            services.AddQuartzHostedService(q =>
            q.WaitForJobsToComplete = true);

            return services;
        }
    }
}