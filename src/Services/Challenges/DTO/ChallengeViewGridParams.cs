using System.Collections.Generic;
using Firdaws.Core;

namespace Tayra.Services
{
    public class ChallengeViewGridParams : GridParams
    {
        public ICollection<int> Segments { get; set; }
    }
}
