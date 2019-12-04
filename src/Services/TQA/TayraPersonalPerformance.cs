using System;
using System.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;

namespace Tayra.Services
{
    public static class TayraPersonalPerformance
    {
        public static void Calc()
        {
            //pull data from tasks
        }

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

        public static int MapPriorityToSeverity(TaskPriorities priority)
        {
            switch (priority)
            {
                case TaskPriorities.Highest:
                    return 5;

                case TaskPriorities.High:
                    return 4;

                case TaskPriorities.Medium:
                    return 3;

                case TaskPriorities.Low:
                    return 2;

                case TaskPriorities.Lowest:
                    return 1;

                default: return 1;
            }
        }
        public static TaskStatuses MapJiraIssueCategoryToTaskStatus(IssueStatusCategories category)
        {
            switch (category)
            {
                case IssueStatusCategories.ToDo:
                    return TaskStatuses.Open;

                case IssueStatusCategories.InProgress:
                    return TaskStatuses.InProgress;

                case IssueStatusCategories.Done:
                    return TaskStatuses.Done;

                default: return TaskStatuses.Open;
            }
        }

    public static double GetProductAreaJuicer(string[] labels)
        {
            if (labels.Contains("Coding"))
            {
                return 2d;
            }
            else if (labels.Contains("Design"))
            {
                return 1.5d;
            }
            else if (labels.Contains("Testing"))
            {
                return 1.3d;
            }

            return 1d;
        }

    }
}
