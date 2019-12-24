using System.Linq;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface ISegmentsService
    {
        IQueryable<Segment> Get();
        Segment Get(int id);
        Segment Get(string segmentKey);
        Segment Create(SegmentCreateDTO dto);
        bool Delete(int id);
    }
}