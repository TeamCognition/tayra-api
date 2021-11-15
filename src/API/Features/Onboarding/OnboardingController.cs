using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Onboarding
{
    public partial class OnboardingController : TayraBaseController
    {
        private readonly ISender _mediator;
        
        public OnboardingController(ISender mediator) => _mediator = mediator;
    }
}