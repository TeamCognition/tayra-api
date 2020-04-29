using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class AssistantMemberGridDTO
    {
        public int ActionPointId { get; set; }
        public ActionPointTypes Type { get; set; }
        public DateTime Created { get; set; }
    }
}
