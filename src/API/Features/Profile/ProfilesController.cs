using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController: TayraBaseController
    {
        private readonly IMediator _mediator;
        
        public ProfilesController(IMediator mediator) => _mediator = mediator;
    }
}