using MediatR;
using Tayra.Common;

namespace Tayra.API.Features.Blobs
{
    public partial class BlobsController: TayraBaseController
    {
        private readonly ISender _mediator;
        
        public BlobsController(ISender mediator) => _mediator = mediator;
    }
}