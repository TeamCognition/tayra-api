using System;
using System.Linq;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class TeamsScopes
    {
        public static IQueryable<Team> TeamsScopeOfSegment(this OrganizationDbContext db, Guid segmentId) => db.Teams.Where(x => x.SegmentId == segmentId);
        public static IQueryable<Team> TeamsScopeOfSegments(this OrganizationDbContext db, Guid[] segmentIds) => db.Teams.Where(x => segmentIds.Contains(x.SegmentId));
    }
}
