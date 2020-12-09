using System;
using Microsoft.Extensions.Logging;

namespace Tayra.SyncServices.Common
{
    public class LogService
    {
        protected string _tenantKey;
        protected Guid _segmentId;
        protected Guid _teamId;
        protected DateTime _timestamp;

        protected ILogger _logger;

        public LogService(ILogger logger)
        {
            _logger = logger;
        }

        public void SetOrganizationId(string tenantKey, DateTime? timestamp = null)
        {
            _tenantKey = tenantKey;
            _timestamp = timestamp ?? _timestamp;
        }

        public void SetSegmentId(Guid segmentId, DateTime? timestamp = null)
        {
            _segmentId = segmentId;
            _timestamp = timestamp ?? _timestamp;
        }

        public void SetTeamId(Guid teamId, DateTime? timestamp = null)
        {
            _teamId = teamId;
            _timestamp = timestamp ?? _timestamp;
        }

        public void SetIds(string tenantKey, Guid segmentId, Guid teamId, DateTime? timestamp = null)
        {
            _tenantKey = tenantKey;
            _segmentId = segmentId;
            _teamId = teamId;
            _timestamp = timestamp ?? DateTime.UtcNow;
        }

        public void Log<T>(string logText, bool isError = false)
        {
            var duration = (DateTime.UtcNow - _timestamp).TotalSeconds;
            var msg = $"{DateTime.UtcNow:O}\t${_tenantKey}.{_segmentId}.{_teamId}\t{typeof(T).Name}:\t{logText} ({duration} seconds)";
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
