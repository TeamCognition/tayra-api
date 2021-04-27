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
            [HttpPut]
            public async Task<Unit> UpdateSegment([FromRoute] Guid segmentId, [FromBody] Update.Command command)
                => await _mediator.Send(command with {SegmentId =  segmentId});
        }

        public class Update
        {
            public record Command : IRequest
            {
                public Guid SegmentId { get; init; }
                public string Key { get; init; }
                public string Name { get; init; }

                public string Avatar { get; init; }
            }

            public class Handler : AsyncRequestHandler<Command>
            {
                private readonly OrganizationDbContext _db;

                public Handler(OrganizationDbContext db) => _db = db;

                protected override async Task Handle(Command msg, CancellationToken token)
                {
                    var segment = await _db.Segments.FirstOrDefaultAsync(x => x.Id == msg.SegmentId, token);
                    segment.EnsureNotNull(msg.SegmentId);

                    if (segment.Key != msg.Key)
                    {
                        var  isSegmentKeyUnique  = !await _db.Segments.AnyAsync(x => x.Key == msg.Key, token);
                        if (isSegmentKeyUnique)
                        {
                            throw new ApplicationException($"A segment exists with the same key");
                        }
                    }

                    segment.Key = msg.Key.Trim();
                    segment.Name = msg.Name.Trim();
                    segment.Avatar = msg.Avatar;
                    
                    await _db.SaveChangesAsync(token);
                }
            }
        }
    }