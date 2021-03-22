using System;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services._Services.Auth
{
    public class AuthService
    {
        public static (IQueryable<Segment>, IQueryable<Team>) GetSegmentAndTeamQueries(OrganizationDbContext dbContext, Guid profileId, ProfileRoles role)
        {
            IQueryable<Segment> qs = dbContext.Segments;
            IQueryable<Team> qt = dbContext.Teams;

            if (role != ProfileRoles.Admin)
            {
                var segmentIds = dbContext.ProfileAssignments.Where(x => x.ProfileId == profileId).Select(x => x.SegmentId).Distinct().ToArray();
                qs = qs.Where(x => segmentIds.Contains(x.Id));

                if (role == ProfileRoles.Manager)
                {
                    qt = qt.Where(x => segmentIds.Contains(x.SegmentId));
                }
                else //is non-admin and non-manager. Is Member
                {
                    var teamIds = dbContext.ProfileAssignments.Where(x => x.ProfileId == profileId).Select(x => x.TeamId).ToArray();
                    qt = qt.Where(x => teamIds.Contains(x.Id));
                }
            }

            return (qs, qt);
        }
    }
}