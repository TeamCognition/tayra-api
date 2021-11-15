using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController: TayraBaseController
    {
        private readonly ISender _mediator;
        
        public TeamsController(ISender mediator) => _mediator = mediator;
    }
}