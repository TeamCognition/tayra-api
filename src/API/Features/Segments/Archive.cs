using System;
using System.Threading;
using System.Threading.Tasks;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpDelete]
        public async Task<Unit> Archive([FromQuery] Guid segmentId) =>
            await _mediator.Send(new Archive.Command {SegmentId = segmentId});
    }

    public class Archive
    {
        public record Command : IRequest
        {
            public Guid SegmentId { get; init; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var segment = await _db.Segments.Include(x => x.Teams)
                    .FirstOrDefaultAsync(x => x.Id == msg.SegmentId, token);
                segment.EnsureNotNull(msg.SegmentId);
                _db.Remove(segment);

                foreach (var t in segment.Teams) _db.Remove(t);
                await _db.SaveChangesAsync(token);
            }
        }
    }
}