using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class DemoTokensService : BaseService<OrganizationDbContext>, ITokensService
    {
        #region Constructor

        public DemoTokensService(OrganizationDbContext dbContext) : base(dbContext)
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
                txn.Value -= txn.FinalBalance;
                txn.FinalBalance = 0;
            }

            DbContext.TokenTransactions.Add(txn);

            return txn.FinalBalance;
        }

        #endregion
    }
}