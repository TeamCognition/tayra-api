using System;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpGet("{username}/rawScore")]
        public async Task<GetRawScore.Result> GetRawScore([FromRoute] string username)
            => await _mediator.Send(new GetRawScore.Query { Username = username});
    }
    
    public class GetRawScore
    {
        public record Query : IRequest<Result>
        {
            public string Username { get; init; }
        }

        public record Result
        {
            public int TasksCompleted { get; set; }	
            public int AssistsGained { get; set; }	
            public int TimeWorked { get; set; }	
            public int DaysOnTayra { get; set; }	
            public float TokensEarned { get; set; }	
            public float TokensSpent { get; set; }	
            public int ItemsBought { get; set; }	
            public int QuestsCompleted { get; set; }
        }                
            
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;
            
            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var profile = await _db.Profiles.FirstOrDefaultAsync(x => x.Username == msg.Username, token);	
                profile.EnsureNotNull(msg.Username);

                var metricService = new MetricService(_db);	

                var metrics = metricService.GetMetrics(new []	
                    {	
                        MetricType.TasksCompleted, MetricType.Assists, MetricType.TimeWorked, MetricType.TokensEarned,	
                        MetricType.TokensSpent, MetricType.ItemsBought	
                    }, profile.Id, EntityTypes.Profile, new DatePeriod(new DateTime(2020, 06, 01), DateTime.UtcNow));	
                
                return new Result	
                {	
                    TasksCompleted = (int) metrics[MetricType.TasksCompleted.Value].Value,	
                    AssistsGained = (int) metrics[MetricType.Assists.Value].Value,	
                    TimeWorked = (int) metrics[MetricType.TimeWorked.Value].Value,	
                    TokensEarned = metrics[MetricType.TokensEarned.Value].Value,	
                    TokensSpent = metrics[MetricType.TokensSpent.Value].Value,	
                    ItemsBought = (int) metrics[MetricType.ItemsBought.Value].Value,	
                    QuestsCompleted = 0,	
                    DaysOnTayra = (DateTime.UtcNow - profile.Created).Days	
                };
            }
        }
    }
}