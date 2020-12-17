using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public interface ITokensService
    {
        double CreateTransaction(TokenType tokenType, Guid profileId, double valueToTransfer, TransactionReason reason, ClaimBundleTypes? claimBundleType, DateTime? date = null);
    }
}
