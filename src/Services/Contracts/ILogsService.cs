using System;
using Cog.Core;
using Tayra.Common;
using Tayra.Mailer;

namespace Tayra.Services
{
    public interface ILogsService
    {
        void LogEvent(LogCreateDTO dto);
        void SendLog(Guid profileId, LogEvents logEvent, ITemplateEmailDTO dto);
        GridData<LogGridDTO> GetGridData(LogGridParams gridParams);
    }
}
