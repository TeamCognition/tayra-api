using System.Collections.Generic;

namespace Tayra.Services
{
    public interface IIntegrationsService
    {
        List<IntegrationViewDTO> GetProjectIntegrations(int projectId);
        JiraSettingsViewDTO GetJiraSettingsViewDTO(int projectId);
        void UpdateJiraSettings(int projectId, JiraSettingsUpdateDTO dto);
    }
}
