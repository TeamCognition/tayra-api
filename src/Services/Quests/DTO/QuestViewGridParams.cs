using System.Collections.Generic;
using Cog.Core;

namespace Tayra.Services
{
    public class QuestViewGridParams : GridParams
    {
        public ICollection<int> Segments { get; set; }
    }
}
