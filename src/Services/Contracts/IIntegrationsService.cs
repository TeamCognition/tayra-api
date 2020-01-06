using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IIntegrationsService
    {
        int GetProfileIdByExternalId(string externalId);
        void DeleteSegmentIntegration(int profileId, int segmentId, IntegrationType integrationType);
        List<IntegrationProfileConfigDTO> GetProfileIntegrationsWithPending(int profileId);
        List<IntegrationSegmentViewDTO> GetSegmentIntegrations(string segmentKey);
        JiraSettingsViewDTO GetJiraSettingsViewDTO(int segmentId);
        void UpdateJiraSettings(int segmentId, JiraSettingsUpdateDTO dto);


    }
}
