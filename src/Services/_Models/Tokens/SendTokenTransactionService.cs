using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services.Models.Tokens
{
    public class SendTokenTransactionService
    {
        public async Task<double> Send(OrganizationDbContext db, TokenType tokenType, Guid profileId, double valueToTransfer, TransactionReason reason, ClaimBundleTypes? claimBundleType,  CancellationToken token, DateTime? date = null)
        {
            var currentBalance = await db.TokenTransactions.Where(x => x.ProfileId == profileId && x.TokenType == tokenType).SumAsync(x => x.Value, token);
            var finalBalance = currentBalance + valueToTransfer;
            if (finalBalance < 0)
            {
                throw new ApplicationException("Not enough tokens to perform the transaction");
            }
            
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

            await db.TokenTransactions.AddAsync(txn, token);

            if (valueToTransfer > 0 && reason == TransactionReason.JiraIssueCompleted)
            {
                //UpdateCompetitorsTokenValue(txn);
            }

            if (claimBundleType.HasValue)
            {
                txn.ClaimRequired = true;
                db.GetTrackedClaimBundle(profileId, claimBundleType.Value).AddTokenTxns(txn);
            }

            return finalBalance;
        }
    }
}