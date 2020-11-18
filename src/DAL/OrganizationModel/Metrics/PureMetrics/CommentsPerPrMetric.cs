using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.VisualBasic.CompilerServices;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Utils = Tayra.Common.Utils;

namespace Tayra.SyncServices.Metrics
{
    public class CommentsPerPrMetric:PureMetric
    {
       public CommentsPerPrMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard[] Create(IEnumerable<PullRequestReviewComment> comments, int dateId ,IEnumerable<PullRequest> pullRequests)
        {
            List<MetricShard> metricShards=null;
            foreach (var pullRequest in pullRequests)
            {
                var commentsPerPr = comments.Where(c => c.PullRequestId == pullRequest.Id);
                if (!Utils.IsNullOrEmpty(commentsPerPr))
                {
                    metricShards.Add(new MetricShard(commentsPerPr.Count(), dateId,
                        this));
                }
            }
            return metricShards.ToArray();
        }
            

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, int entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from c in db.PullRequestReviewComments
                where profileIds.Contains(c.PullRequestId)
                where c.Created >= period.From && c.Created < period.To
                select new RawMetric
                {
                    Author = new TableData.Profile(
                        $"{c.CommenterProfile.FirstName} {c.CommenterProfile.LastName}",
                        c.CommenterProfile.Username),
                    Comment = new TableData.ExternalLink(c.Body, c.ExternalUrl),
                    Date = new TableData.DateInSeconds(c.Created)
                }).ToArray<object>();

        }
        
        public override Type TypeOfRawMetric => typeof(RawMetric);


        public class RawMetric
        {
            public TableData.Profile Author { get; set; }
            public TableData.ExternalLink Comment { get; set; }
            public TableData.DateInSeconds Date { get; set; }
        }
    }
}