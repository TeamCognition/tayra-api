using Firdaws.Core;

namespace Tayra.Services
{
    public interface ILogsService
    {
        void LogEvent(LogCreateDTO dto);
        GridData<LogGridDTO> GetGridData(LogGridParams gridParams);
    }
}
