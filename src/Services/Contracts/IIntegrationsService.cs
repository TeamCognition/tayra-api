using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IIntegrationsService
    {
        void SetProfileIntegration(int profileId, IntegrationProfileConfigDTO dto);
        void DeleteProfileIntegration(int profileId, int projectId, IntegrationType integrationType);
        List<IntegrationProfileConfigDTO> GetProfileIntegrationsWithPending(int profileId);
        List<IntegrationProjectViewDTO> GetProjectIntegrations(int projectId);
        JiraSettingsViewDTO GetJiraSettingsViewDTO(int projectId);
        void UpdateJiraSettings(int projectId, JiraSettingsUpdateDTO dto);


    }
}
