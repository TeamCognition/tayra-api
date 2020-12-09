using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
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

        public double CreateTransaction(TokenType tokenType, Guid profileId, double value, TransactionReason reason, ClaimBundleTypes? claimBundleType, DateTime? date = null)
        {
            var tokenId = DbContext.Tokens.FirstOrDefault(x => x.Type == tokenType);
            if (tokenId == null)
            {
                throw new ApplicationException("No token of type " + tokenType);
            }

            return CreateTransaction(tokenId.Id, profileId, value, reason, claimBundleType, date);
        }

        public double CreateTransaction(Guid tokenId, Guid profileId, double value, TransactionReason reason, ClaimBundleTypes? claimBundleType, DateTime? date = null)
        {
            var scope = DbContext.TokenTransactions.Where(x => x.ProfileId == profileId && x.TokenId == tokenId);
            var txn = new TokenTransaction
            {
                ProfileId = profileId,
                Reason = reason,
                TokenId = tokenId,
                TxnHash = string.Empty,
                Value = value,
                FinalBalance = scope.Sum(x => x.Value) + value,
                ClaimRequired = false,
                DateId = DateHelper2.ToDateId(date ?? DateTime.UtcNow)
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
    }
}