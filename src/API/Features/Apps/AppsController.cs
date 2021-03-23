using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Apps
{
    public partial class AppsController: TayraBaseController
    {
        private readonly ISender _mediator;
        
        public AppsController(ISender mediator) => _mediator = mediator;
    }
}