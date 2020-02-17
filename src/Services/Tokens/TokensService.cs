using System;
using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class TokensService : BaseService<OrganizationDbContext>, ITokensService
    {
        #region Constructor

        public TokensService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public List<TokenLookupDTO> GetTokenLookupDTO()
        {
            return DbContext.Tokens
                .Select(x => new TokenLookupDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Symbol = x.Symbol,
                    Type = x.Type
                })
                .ToList();
        }

        public double CreateTransaction(TokenType tokenType, int profileId, double value, TransactionReason reason, ClaimBundleTypes? claimBundleType, string txnHash = "")
        {
            var tokenId = DbContext.Tokens.FirstOrDefault(x => x.Type == tokenType);
            if (tokenId == null)
            {
                throw new ApplicationException("No token of type " + tokenType);
            }

            return CreateTransaction(tokenId.Id, profileId, value, reason, claimBundleType, txnHash);
        }

        public double CreateTransaction(int tokenId, int profileId, double value, TransactionReason reason, ClaimBundleTypes? claimBundleType, string txnHash = "")
        {
            var scope = DbContext.TokenTransactions.Where(x => x.ProfileId == profileId && x.TokenId == tokenId);
            var txn = new TokenTransaction
            {
                ProfileId = profileId,
                Reason = reason,
                TokenId = tokenId,
                TxnHash = txnHash,
                Value = value,
                FinalBalance = scope.Sum(x => x.Value) + value,
                ClaimRequired = false
            };

            if (txn.FinalBalance < 0)
            {
                throw new ApplicationException("Not enough tokens to perform the transaction");
            }

            DbContext.TokenTransactions.Add(txn);

            if (value > 0 && reason == TransactionReason.JiraIssueCompleted)
            {
                //UpdateCompetitorsTokenValue(txn);
            }

            if (claimBundleType.HasValue)
            {
                txn.ClaimRequired = true;
                DbContext.GetTrackedClaimBundle(profileId, claimBundleType.Value).AddTokenTxns(txn);
            }

            return txn.FinalBalance;
        }

        #endregion

        #region Private Methods

        private void UpdateCompetitorsTokenValue(TokenTransaction txn)
        {
            var teamIds = DbContext.ProfileAssignments
                .Where(x => x.ProfileId == txn.ProfileId)
                .Select(x => x.TeamId)
                .ToList();

            var competitors = DbContext.Competitors
                .Where(x => (x.ProfileId.HasValue && x.ProfileId == txn.ProfileId) || (x.TeamId.HasValue && teamIds.Contains(x.TeamId.Value)))
                .Where(x => x.Competition.Status == CompetitionStatus.Started)
                .Where(x => x.Competition.TokenId == txn.TokenId)
                .ToList();


            foreach (var c in competitors)
            {
                c.ScoreValue += txn.Value;

                DbContext.Add(new CompetitorScore
                {
                    CompetitorId = c.Id,
                    ProfileId = txn.ProfileId,
                    Value = txn.Value,
                    TeamId = c.TeamId,
                    CompetitionId = c.CompetitionId
                });
            }
        }

        #endregion

    }
}