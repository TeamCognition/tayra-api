﻿using System.Linq;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class TeamsScopes
    { 
        public static IQueryable<Team> TeamsScopeCommon (this OrganizationDbContext db) => db.Teams.Where(x => x.Key != null);
        public static IQueryable<Team> TeamsScopeOfSegment (this OrganizationDbContext db, int segmentId) => db.TeamsScopeCommon().Where(x => x.SegmentId == segmentId);
        public static IQueryable<Team> TeamsScopeOfSegments(this OrganizationDbContext db, int[] segmentIds) => db.TeamsScopeCommon().Where(x => segmentIds.Contains(x.SegmentId));
    }
}
