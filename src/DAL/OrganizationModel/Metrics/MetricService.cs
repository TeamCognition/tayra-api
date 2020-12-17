using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Connectors.GitHub.Common;
using Tayra.Models.Organizations.Metrics.GraphqlTypes;
using Tayra.Models.Organizations.Metrics.ResponseModels;
using Tayra.Services;

namespace Tayra.Models.Organizations.Metrics
{
    public class MetricService
    {
        public MetricService(OrganizationDbContext db)
        {
            _db = db;
        }

        private readonly OrganizationDbContext _db;
        
        private const string GRAPHQL_URL = "https://api.github.com/graphql";
        private const string REPOSITORY_OWNER = "TeamCognition";
        private const string REPOSITORY_NAME = "tayra-api";
        private const string GITHUB_INSTALATION_ID = "GHInstallationId";
        private const string GITUB_ACCESS_TOKEN = "AccessToken";

        public class AnalyticsMetricWithIterationSplitDto
        {
            public DatePeriod Period { get; }
            public float Value { get; }
            public IterationDto[] Iterations { get; }

            public AnalyticsMetricWithIterationSplitDto(MetricType metricType, DatePeriod period, MetricShard[] raws, EntityTypes entityType)
            {
                this.Period = period;
                this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
                this.Iterations = period.SplitToIterations().Select(i => new IterationDto(metricType, i, raws, entityType)).ToArray();
            }

            public class IterationDto
            {
                public DatePeriod Period { get; set; }
                public float Value { get; set; }

                public IterationDto(MetricType metricType, DatePeriod iterationPeriod, MetricShard[] raws, EntityTypes entityType)
                {
                    Period = iterationPeriod;
                    this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, iterationPeriod) : metricType.CalcGroup(raws, iterationPeriod);
                }
            }
        }
        
        public class AnalyticsMetricWithBreakdownDto
        {
            public DateTime? LastRefreshAt { get; set; }
            public DatePeriod Period { get; set; }
            public float Value { get; set; }
            public IterationBreakdownDto[] IterationsBreakdown { get; set; }

            public AnalyticsMetricWithBreakdownDto(MetricType metricType, DatePeriod period, MetricShard[] raws, DateTime lastRefreshAt, EntityTypes entityType)
            {
                this.LastRefreshAt = lastRefreshAt;
                this.Period = period;
                this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
                this.IterationsBreakdown = period.SplitToIterations().Select(i => new IterationBreakdownDto(metricType.BuildingMetrics.Append(metricType).ToArray(), i, raws, entityType)).ToArray();
            }

            public class IterationBreakdownDto
            {
                public DatePeriod Period { get; set; }
                public Dictionary<int, float> Metrics { get; set; }

                public IterationBreakdownDto(MetricType[] types, DatePeriod iterationPeriod, MetricShard[] raws, EntityTypes entityType)
                {
                    Period = iterationPeriod;
                    Metrics = entityType == EntityTypes.Profile ? types.ToDictionary(t => t.Value, t => t.Calc(raws, iterationPeriod)) : types.ToDictionary(t => t.Value, t => t.CalcGroup(raws, iterationPeriod));
                }
            }
        }
        
        
        public Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> GetMetricsWithIterationSplit(MetricType[] metricTypes, Guid entityId, EntityTypes entityType, DatePeriod period)
        {
            var rawMetrics = metricTypes.Concat(metricTypes.SelectMany(x => x.BuildingMetrics)).ToArray();
            rawMetrics = rawMetrics.Concat(rawMetrics.SelectMany(x => x.BuildingMetrics)).ToArray();

            MetricShard[] metrics = null;

            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in _db.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.SegmentId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricShard
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
                default:
                    metrics = (from m in _db.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.ProfileId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricShard
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }

            return metricTypes.ToDictionary(type => type.Value,
                type => new MetricService.AnalyticsMetricWithIterationSplitDto(type, period, metrics, entityType));
        }
        
        public Dictionary<int, MetricValue> GetMetrics(MetricType[] metricsTypes, Guid entityId, EntityTypes entityType, DatePeriod period)
        {
            var rawMetrics = metricsTypes.Concat(metricsTypes.SelectMany(x => x.BuildingMetrics)).ToArray();
            rawMetrics = rawMetrics.Concat(rawMetrics.SelectMany(x => x.BuildingMetrics)).ToArray();

            MetricShard[] metrics = null;

            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in _db.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.SegmentId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricShard
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
                default:
                    metrics = (from m in _db.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.ProfileId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricShard
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }

            return metricsTypes.ToDictionary(type => type.Value,
                type => new MetricValue(type, period, metrics, entityType));
        }

        public Dictionary<int, MetricsValueWEntity[]> GetMetricsRanks(MetricType[] metricTypes, Guid[] entityIds, EntityTypes entityType, DatePeriod period)
        {
            MetricShardWEntity[] metrics = null;

            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in _db.SegmentMetrics
                               where m.DateId >= period.FromId && m.DateId <= period.ToId
                               where entityIds.Contains(m.SegmentId)
                               select new MetricShardWEntity
                               {
                                   EntityId = m.SegmentId,
                                   Type = m.Type,
                                   Value = m.Value,
                                   DateId = m.DateId
                               }).ToArray();
                    break;
                case EntityTypes.Team:
                    metrics = (from m in _db.TeamMetrics
                               where m.DateId >= period.FromId && m.DateId <= period.ToId
                               where entityIds.Contains(m.TeamId)
                               select new MetricShardWEntity
                               {
                                   EntityId = m.TeamId,
                                   Type = m.Type,
                                   Value = m.Value,
                                   DateId = m.DateId
                               }).ToArray();
                    break;
                default:
                    metrics = (from m in _db.ProfileMetrics
                               where m.DateId >= period.FromId && m.DateId <= period.ToId
                               where entityIds.Contains(m.ProfileId)
                               select new MetricShardWEntity
                               {
                                   EntityId = m.ProfileId,
                                   Type = m.Type,
                                   Value = m.Value,
                                   DateId = m.DateId
                               }).ToArray();
                    break;
            }

            return metricTypes.ToDictionary(type => type.Value,
                type => entityIds.Select(eId => MetricsValueWEntity.Create(type, eId, period, metrics, entityType)).OrderByDescending(x => x.Value).ToArray());
        }
        
        public static List<CommitType> GetCommitsByPUllRequest(string tokenType, string token,int _pullRequestNumber)
        {
            using var graphQLClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");
            var commitsRequest = new GraphQLRequest
            {
                Query = @"
                query CommitsByPullRequest($repositoryName : String!, $repositoryOwner : String!,$pullRequestNumber : Int!){
                    repository(name:$repositoryName, owner:$repositoryOwner){
                pullRequest(number:$pullRequestNumber){
                commits(first:100){
                edges{
                node{
                commit{
                oid,
                author{
                     name,email
                },
                message,
                additions,
                deletions
              
            }
            }
            }
            }
            }
            }
}
                    ",
                OperationName = "CommitsByPullRequest",
                Variables = new {
                    repositoryOwner = REPOSITORY_OWNER,
                    repositoryName = REPOSITORY_NAME,
                    pullRequestNumber = _pullRequestNumber
                }
            };
            var gitGraphQlResponse =
                graphQLClient.SendQueryAsync<GitCommitByPrResponse>(commitsRequest).GetAwaiter().GetResult();
            List<Edge<GitCommitByPrResponse.PullRequestCommit>> responseCommits =
                gitGraphQlResponse.Data.Repository.pullRequest.Commits.edges;
            List<CommitType> commitsForRes = new List<CommitType>();
            foreach (var edge in responseCommits)
            {
                commitsForRes.Add(edge.Node.Commit);
            }
            return commitsForRes;
        }

        public Guid GetIntegrationId(IntegrationType type)
        {
            if (type == IntegrationType.GH)
            {
                var githubIntegrationField = _db.IntegrationFields
                    .FirstOrDefault(x => x.Key == GITHUB_INSTALATION_ID);
                if (githubIntegrationField == null)
                {
                    throw new ApplicationException("Integration not found for GH");
                }
                return githubIntegrationField.IntegrationId;
            }
            throw new ArgumentException("Integration type not supported");
        }

        public string ReadAccessToken(Guid integrationId)
        {
            var field = _db.IntegrationFields.FirstOrDefault(a => a.IntegrationId == integrationId && a.Key == GITUB_ACCESS_TOKEN);

            if (string.IsNullOrWhiteSpace(field?.Value))
            {
                throw new ApplicationException("Unable to access the integration account, access token is missing or has expired");
            }

            return field?.Value;
        }
    }
}