using System.Collections.Generic;
using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class ChallengeViewGridParams : GridParams
    {
        public IList<ChallengeStatuses> Statuses { get; set; }
    }
}
