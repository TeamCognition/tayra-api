using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController : TayraBaseController
    {
        private readonly ISender _mediator;
        
        public ProfilesController(ISender mediator) => _mediator = mediator;
    }
}