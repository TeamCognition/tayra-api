using Tayra.Common;
using Tayra.Services.TaskConverters;

namespace Tayra.Services
{
    public static class TaskHelpers
    {
        public static TaskPriorities GetTaskPriority(string jiraIssuePriorityId)
        {
            switch (jiraIssuePriorityId)
            {
                case "1":
                    return TaskPriorities.Highest;

                case "2":
                    return TaskPriorities.High;

                case "3":
                    return TaskPriorities.Medium;

                case "4":
                    return TaskPriorities.Low;

                case "5":
                    return TaskPriorities.Lowest;

                default: return TaskPriorities.Medium;
            }
        }

        public static TaskTypes GetTaskType(string jiraIssueTypeId)
        {
            switch (jiraIssueTypeId)
            {
                case "10006":
                    return TaskTypes.Bug;

                default: return TaskTypes.Task;
            }
        }

        public static bool DoStandardStuff(TaskConverterBase taskConverter,
                                           ITasksService tasksService,
                                           ITokensService tokensService,
                                           ILogsService logsService,
                                           IAdvisorService advisorService)
        {
            taskConverter.UpdateBasicTaskData();
            if (taskConverter.ShouldBeProcessed())
            {
                taskConverter.FillExtraDataIfCompleted();
                taskConverter.AddNecessaryTokensIfPossible(tokensService);
                taskConverter.ConcludeActionPointsIfPossible(advisorService);
                taskConverter.LogIfPossible(logsService);
                tasksService.AddOrUpdate(taskConverter.Data);
                return true;
            }
            return false;
        }
    }
}
