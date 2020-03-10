using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IAdvisorService
    {
        GridData<ActionPointGridDTO> GetActionPointGrid(GridParams gridParams);
    }
}