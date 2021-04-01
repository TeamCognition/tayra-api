using System;
using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class AtlassianJiraWkUnStatusesConfiguration
    {
        public AtlassianJiraWkUnStatusesConfiguration(string projectId )
        {
            ProjectId = projectId;
        }
        
        //todo: add rest of props
        public string ProjectId { get; set; }
        public List<WorkUnitStatusConfiguration> WorkUnitStatuses { get; set; }

        public static AtlassianJiraWkUnStatusesConfiguration From(string projectId, IEnumerable<IntegrationField> fields)
        {
            AtlassianJiraWkUnStatusesConfiguration config = new(projectId);
            
            var statusFields = fields.Where(x => x.Key == ATConstants.PM_WORK_UNIT_STATUS_FOR_PROJECT_ + projectId).ToArray();
            
            config.WorkUnitStatuses.AddRange(
                statusFields.Select(x => new WorkUnitStatusConfiguration(x.Value)).ToArray());
            
            return config;
        }

        public static AtlassianJiraWkUnStatusesConfiguration From(AppsProjectConfig projectConfig)
        {
            AtlassianJiraWkUnStatusesConfiguration config = new(projectConfig.ProjectId);
            
            config.WorkUnitStatuses.AddRange(
                projectConfig.Statuses.Select(x => new WorkUnitStatusConfiguration(x.ExternalStatusId, x.Status)).ToArray());

            return config;
        }

        public IEnumerable<IntegrationField> ExportToIntegrationFields()
        {
            return WorkUnitStatuses.Select(status => 
                new IntegrationField { Key = ATConstants.PM_WORK_UNIT_STATUS_FOR_PROJECT_ + ProjectId, Value = status.ToString() });
        }

        public WorkUnitStatuses GetStatusByExternalStatusId(string externalStatusId)
        {
            return WorkUnitStatuses.First(x => x.ExternalStatusId == externalStatusId).Status;
        }
        
        public class WorkUnitStatusConfiguration
        {
            public string ExternalStatusId { get; set; }
            public WorkUnitStatuses Status { get; set; }

            public WorkUnitStatusConfiguration(string externalStatusId, WorkUnitStatuses status)
            {
                ExternalStatusId = externalStatusId;
                Status = status;
            }
            
            public WorkUnitStatusConfiguration(string config)
            {
                try
                {
                    var props = SplitProps(config);
                    ExternalStatusId = props[0];
                    Status = Enum.Parse<WorkUnitStatuses>(props[1]);
                }
                catch (Exception)
                {
                    throw new ApplicationException("could not be parsed");
                }
            }

            public override string ToString() => JoinProps();
            
            private const char _separator = '|';
            private string JoinProps() => string.Join(_separator, ExternalStatusId, Status);
            private string[] SplitProps(string joined) => joined.Split(_separator);
        }
    }
}