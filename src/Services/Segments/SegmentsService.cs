using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class SegmentsService : BaseService<OrganizationDbContext>, ISegmentsService
    {
        #region Constructor

        public SegmentsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public IQueryable<Segment> Get()
        {
            return DbContext.Segments
                .AsNoTracking();
        }

        public Segment Get(int id)
        {
            return Get()
                .FirstOrDefault(i => i.Id == id);
        }

        public Segment Get(string segmentKey)
        {
            return Get()
                .FirstOrDefault(i => i.Key == segmentKey);
        }

        public Segment Create(SegmentCreateDTO dto)
        {
            var segment = new Segment
            {
                Key = dto.Key,
                Name = dto.Name,
                Timezone = dto.Timezone,
                OrganizationId = 1
            };

            DbContext.Segments.Add(segment);
            DbContext.SaveChanges();
            return segment;
        }

        public bool Delete(int id)
        {
            var segmentToDelete = GetOrFail(id);
            DbContext.Segments.Remove(segmentToDelete);
            var affectedRecords = DbContext.SaveChanges();
            return affectedRecords > 0;
        }

        #endregion

        private Segment GetOrFail(int id)
        {
            var item = Get(id);
            if (item == null)
            {
                throw new ApplicationException($"{typeof(Segment)} does not exist.");
            }

            return item;
        }
    }
}
