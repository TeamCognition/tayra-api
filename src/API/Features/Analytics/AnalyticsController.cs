using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Analytics
{
    public partial class AnalyticsController : TayraBaseController
    {
        private readonly IMediator _mediator;
        
        public AnalyticsController(IMediator mediator) => _mediator = mediator;
    }
}