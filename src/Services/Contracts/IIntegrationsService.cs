using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IIntegrationsService
    {
        int GetProfileIdByExternalId(string externalId);
        void DeleteSegmentIntegration(int profileId, int segmentId, IntegrationType integrationType);
        List<IntegrationProfileConfigDTO> GetProfileIntegrationsWithPending(int[] segmentIds, int profileId);
        List<IntegrationSegmentViewDTO> GetSegmentIntegrations(string segmentKey);
        JiraSettingsViewDTO GetJiraSettingsViewDTO(string webhookServerUrl, string tenantKey, int segmentId);
        void UpdateJiraSettingsWithSaveChanges(int segmentId, string organizationKey, JiraSettingsUpdateDTO dto);
    }
}
