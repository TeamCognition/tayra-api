using System;
using Microsoft.Extensions.Logging;

namespace Tayra.SyncServices.Common
{
    public class LogService
    {
        protected int _organizationId;
        protected int _projectId;
        protected int _teamId;
        protected DateTime _timestamp;

        protected ILogger _logger;

        public LogService(ILogger logger)
        {
            _logger = logger;
        }

        public void SetOrganizationId(int organizationId, DateTime? timestamp = null)
        {
            _organizationId = organizationId;
             _timestamp = timestamp ?? _timestamp;
        }

        public void SetProjectId(int projectId, DateTime? timestamp = null)
        {
            _projectId = projectId;
            _timestamp = timestamp ?? _timestamp;
        }

        public void SetTeamId(int teamId, DateTime? timestamp = null)
        {
            _teamId = teamId;
             _timestamp = timestamp ?? _timestamp;
        }

        public void SetIds(int organizationId, int projectId, int teamId, DateTime? timestamp = null)
        {
            _organizationId = organizationId;
            _projectId = projectId;
            _teamId = teamId;
            _timestamp = timestamp ?? DateTime.UtcNow;
        }

        public void Log<T>(string logText, bool isError = false)
        {
            var duration = (DateTime.UtcNow - _timestamp).TotalSeconds;
            var msg = $"{DateTime.UtcNow:O}\t${_organizationId}.{_projectId}.{_teamId}\t{typeof(T).Name}:\t{logText} ({duration} seconds)";
            _timestamp = DateTime.UtcNow;

            if (isError)
            {
                _logger.LogError(msg);
            }
            else
            {
                _logger.LogInformation(msg);
            }
        }
    }
}
