﻿using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Common
{
    public interface IOAuthConnector : IConnector
    {
        string GetAuthUrl(OAuthState state);

        string GetAuthDoneUrl(string returnPath, bool isSuccessful);

        Integration Authenticate(OAuthState state);

        Integration RefreshToken(int segmentId);
    }
}