using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Common
{
    public interface IOAuthConnector : IConnector
    {
        string GetAuthUrl(string userState);

        string GetAuthDoneUrl(string returnPath, bool isSuccessful);

        Integration Authenticate(int profileId, ProfileRoles profileRole, int segmentId, string userState);

        Integration RefreshToken(int segmentId);
    }
}