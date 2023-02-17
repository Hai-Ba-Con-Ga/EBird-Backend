using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Services.IServices;
using Quartz;

namespace EBird.Api.QuartzJob
{
    [DisallowConcurrentExecution]
    public class MatchingJob : IJob
    {
        public MatchingJob(IScoringService scoringService)
        {
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Matching Job working...");
        }
    }
}