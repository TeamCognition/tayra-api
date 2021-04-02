using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController : TayraBaseController
    {
        private readonly ISender _mediator;
        public SegmentsController(ISender mediator) => _mediator = mediator;
    }
}