using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Tayra.Common;
using Tayra.Connectors.GitHub.Common;
using Tayra.Models.Organizations.Metrics.GraphqlTypes;
using Tayra.Models.Organizations.Metrics.ResponseModels;

namespace Tayra.Models.Organizations.Metrics
{
    public class MetricService
    {
        public MetricService(OrganizationDbContext organizationDbContext)
        {
            _organizationDbContext = organizationDbContext;
        }

        private readonly OrganizationDbContext _organizationDbContext;
        
        private const string GRAPHQL_URL = "https://api.github.com/graphql";
        private const string REPOSITORY_OWNER = "TeamCognition";
        private const string REPOSITORY_NAME = "tayra-api";
        private const string GITHUB_INSTALATION_ID = "GHInstallationId";
        private const string GITUB_ACCESS_TOKEN = "AccessToken";

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

        public int GetIntegrationId(IntegrationType type)
        {
            if (type == IntegrationType.GH)
            {
                var githubIntegrationField = _organizationDbContext.IntegrationFields
                    .FirstOrDefault(x => x.Key == GITHUB_INSTALATION_ID);
                if (githubIntegrationField == null)
                {
                    throw new ApplicationException("Integration not found for GH");
                }
                return githubIntegrationField.IntegrationId;
            }
            throw new ArgumentException("Integration type not supported");
        }

        public string ReadAccessToken(int integrationId)
        {
            var field = _organizationDbContext.IntegrationFields.FirstOrDefault(a => a.IntegrationId == integrationId && a.Key == GITUB_ACCESS_TOKEN);

            if (string.IsNullOrWhiteSpace(field?.Value))
            {
                throw new ApplicationException("Unable to access the integration account, access token is missing or has expired");
            }

            return field?.Value;
        }
    }
}