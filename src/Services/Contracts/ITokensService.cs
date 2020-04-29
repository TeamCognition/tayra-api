using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public interface ITokensService
    {
        List<TokenLookupDTO> GetTokenLookupDTO();
        double CreateTransaction(int tokenId, int profileId, double value, TransactionReason reason, ClaimBundleTypes? claimBundleType, DateTime? date = null);
        double CreateTransaction(TokenType tokenType, int profileId, double value, TransactionReason reason, ClaimBundleTypes? claimBundleType, DateTime? date = null);
    }
}
