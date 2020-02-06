using System.Collections.Generic;

namespace Tayra.Services
{
    public interface IReportsService
    {
        ReportOverviewDTO GetOverviewReport(ReportParams reportParams);
    }
}
