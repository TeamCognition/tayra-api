using System;
using System.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class TayraPersonalPerformance
    {
        public static int MapSPToComplexity(int? storyPoints)
        {
            if (!storyPoints.HasValue)
                return 1;

            switch (storyPoints)
            {
                case 2:
                    return 2;

                case 3:
                case 4:
                    return 3;

                case 5:
                case 6:
                case 7:
                    return 4;

                case 8:
                    return 5;

                default:
                    if (storyPoints > 8)
                        return 5;
                    return 1;
            }
        }

        public static int MapPriorityToSeverity(WorkUnitPriorities priority)
        {
            switch (priority)
            {
                case WorkUnitPriorities.Highest:
                    return 5;

                case WorkUnitPriorities.High:
                    return 4;

                case WorkUnitPriorities.Medium:
                    return 3;

                case WorkUnitPriorities.Low:
                    return 2;

                case WorkUnitPriorities.Lowest:
                    return 1;

                default: return 1;
            }
        }
    }
}
