using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Tenants
{
    public partial class TenantsController : TayraBaseController
    {
        private readonly ISender _mediator;
        
        public TenantsController(ISender mediator) => _mediator = mediator;
    }
}