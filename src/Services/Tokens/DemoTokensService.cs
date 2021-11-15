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
        
        public double CreateTransaction(TokenType tokenType, Guid profileId, double valueToTransfer, TransactionReason reason, ClaimBundleTypes? claimBundleType, DateTime? date = null)
        {
            var currentBalance = DbContext.TokenTransactions.Where(x => x.ProfileId == profileId && x.TokenType == tokenType).Sum(x => x.Value);
            var finalBalance = currentBalance + valueToTransfer;

            var txn = new TokenTransaction
            {
                ProfileId = profileId,
                Reason = reason,
                TokenType = tokenType,
                TxnHash = string.Empty,
                Value = valueToTransfer,
                ClaimRequired = false,
                DateId = DateHelper2.ToDateId(date ?? DateTime.UtcNow)
            };

            DbContext.TokenTransactions.AddAsync(txn);

            if (valueToTransfer > 0 && reason == TransactionReason.JiraIssueCompleted)
            {
                //UpdateCompetitorsTokenValue(txn);
            }

            if (claimBundleType.HasValue)
            {
                txn.ClaimRequired = true;
                DbContext.GetTrackedClaimBundle(profileId, claimBundleType.Value).AddTokenTxns(txn);
            }

            return finalBalance;
        }

        #endregion
    }
}