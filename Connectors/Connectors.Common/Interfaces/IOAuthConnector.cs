using Tayra.Models.Organizations;

namespace Tayra.Connectors.Common
{
    public interface IOAuthConnector : IConnector
    {
        string GetAuthUrl(string userState);

        string GetAuthDoneUrl(bool isSuccessful);

        Integration Authenticate(int projectId, string userState);

        Integration RefreshToken(int projectId);
    }
}