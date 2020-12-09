﻿using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IIntegrationsService
    {
        Guid GetProfileIdByExternalId(string externalId);
        void DeleteSegmentIntegration(Guid profileId, Guid segmentId, IntegrationType integrationType);
        List<IntegrationProfileConfigDTO> GetProfileIntegrationsWithPending(Guid[] segmentIds, Guid profileId);
        List<IntegrationSegmentViewDTO> GetSegmentIntegrations(string segmentKey);
        JiraSettingsViewDTO GetJiraSettingsViewDTO(string webhookServerUrl, string tenantKey, Guid segmentId);
        void UpdateJiraSettingsWithSaveChanges(Guid segmentId, string organizationKey, JiraSettingsUpdateDTO dto);
    }
}
