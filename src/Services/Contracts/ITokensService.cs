using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public interface ITokensService
    {
        List<TokenLookupDTO> GetTokenLookupDTO();
        double CreateTransaction(Guid tokenId, Guid profileId, double value, TransactionReason reason, ClaimBundleTypes? claimBundleType, DateTime? date = null);
        double CreateTransaction(TokenType tokenType, Guid profileId, double value, TransactionReason reason, ClaimBundleTypes? claimBundleType, DateTime? date = null);
    }
}
