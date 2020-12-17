using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tayra.Models.Organizations;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpGet("activityChart/{profileId:int}")]
        public async Task<GetActivityChart.Result[]> GetActivityChart([FromUri] Guid profileId)
            => await _mediator.Send(new GetActivityChart.Query { ProfileId = CurrentUser.ProfileId});
    }
    public class GetActivityChart
    {
        public record Query : IRequest<Result[]>
        {
            public Guid ProfileId { get; init; }
        }

        public record Result
        {
            public int DateId { get; set; }
            public AssistsDTO AssistsData { get; set; }
            public DeliveryDTO DeliveryData { get; set; }
            public ItemActivityDTO ItemActivityData { get; set; }
            //public QuestDTO QuestsData { get; set; }

            public GitCommitDTO[] GitCommitData { get; set; }

            public record AssistsDTO
            {
                public string[] EndorsedBy { get; set; }
                public string[] Endorsed { get; set; }
            }

            public record DeliveryDTO
            {
                public string[] TaskName { get; set; }
                public double TokensGained { get; set; }
            }

            public record ItemActivityDTO
            {
                public string[] Bought { get; set; }
                public string[] Disenchanted { get; set; }
                public string[] GiftsReceived { get; set; }
                public string[] GiftsSent { get; set; }
            }

            public record QuestsDTO
            {
                public string[] CommittedTo { get; set; }
                public string[] GoalsCompleted { get; set; }
                public string[] Completed { get; set; }
            }

            public record GitCommitDTO
            {
                public string Message { get; set; }
                public string ExternalUrl { get; set; }
            }
        }
        
        public class Handler : IRequestHandler<Query, Result[]>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result[]> Handle(Query msg, CancellationToken token)
            {
                var oldestDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-31));
                await Task.Delay(1, token);
                return (from r in _db.ProfileReportsDaily
                    where r.ProfileId == msg.ProfileId && r.ActivityChartJson != null
                    where r.DateId >= oldestDateId
                    orderby r.DateId
                    select new
                    {
                        DateId = r.DateId,
                        ActivityChart = JsonConvert.DeserializeObject<ProfileActivityChartDTO>(r.ActivityChartJson)
                    }).ToArray()
                    .GroupBy(x => x.DateId)
                    .Select(g => new Result
                    {
                        DateId = g.Key,
                        AssistsData = new Result.AssistsDTO
                        {
                            Endorsed = g.SelectMany(x => x.ActivityChart.AssistsData?.Endorsed ?? new string[0]).ToArray(),
                            EndorsedBy = g.SelectMany(x => x.ActivityChart.AssistsData?.EndorsedBy ?? new string[0]).ToArray()
                        },
                        DeliveryData = new Result.DeliveryDTO
                        {
                            TaskName = g.SelectMany(x => x.ActivityChart.DeliveryData?.TaskName ?? new string[0]).ToArray(),
                            TokensGained = g.Sum(x => x.ActivityChart.DeliveryData.TokensGained),
                        },
                        ItemActivityData = new Result.ItemActivityDTO
                        {
                            Bought = g.SelectMany(x => x.ActivityChart.ItemActivityData?.Bought ?? new string[0]).ToArray(),
                            Disenchanted = g.SelectMany(x => x.ActivityChart.ItemActivityData?.Disenchanted ?? new string[0]).ToArray(),
                            GiftsReceived = g.SelectMany(x => x.ActivityChart.ItemActivityData?.GiftsReceived ?? new string[0]).ToArray(),
                            GiftsSent = g.SelectMany(x => x.ActivityChart.ItemActivityData?.GiftsSent ?? new string[0]).ToArray(),
                        },
                        GitCommitData = g.Any(x => x.ActivityChart.GitCommitData != null) ? g.SelectMany(x => x.ActivityChart?.GitCommitData?.Select(c =>
                         new Result.GitCommitDTO
                         {
                             Message = c?.Message ?? string.Empty,
                             ExternalUrl = c?.ExternalUrl ?? string.Empty
                         }))?.ToArray()
                            : new Result.GitCommitDTO[] { }
                    }).ToArray();
            }
        }
    }
}