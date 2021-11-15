using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Analytics
{
    public partial class AnalyticsController : TayraBaseController
    {
        private readonly ISender _mediator;
        
        public AnalyticsController(ISender mediator) => _mediator = mediator;
    }
}