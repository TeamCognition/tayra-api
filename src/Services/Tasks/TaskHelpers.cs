using Tayra.Common;
using Tayra.Services.TaskConverters;

namespace Tayra.Services
{
    public static class TaskHelpers
    {
        public static WorkUnitPriorities GetTaskPriority(string jiraIssuePriorityId)
        {
            switch (jiraIssuePriorityId)
            {
                case "1":
                    return WorkUnitPriorities.Highest;

                case "2":
                    return WorkUnitPriorities.High;

                case "3":
                    return WorkUnitPriorities.Medium;

                case "4":
                    return WorkUnitPriorities.Low;

                case "5":
                    return WorkUnitPriorities.Lowest;

                default: return WorkUnitPriorities.Medium;
            }
        }

        public static WorkUnitTypes GetTaskType(string jiraIssueTypeId)
        {
            switch (jiraIssueTypeId)
            {
                case "10006":
                    return WorkUnitTypes.Bug;

                default: return WorkUnitTypes.Task;
            }
        }

        public static bool DoStandardStuff(TaskConverterBase taskConverter,
                                           ITasksService tasksService,
                                           ITokensService tokensService,
                                           ILogsService logsService,
                                           IAssistantService assistantService)
        {
            taskConverter.UpdateBasicTaskData();
            if (taskConverter.ShouldBeProcessed())
            {
                taskConverter.FillExtraDataIfCompleted();
                taskConverter.AddNecessaryTokensIfPossible(tokensService);
                taskConverter.ConcludeActionPointsIfPossible(assistantService);
                taskConverter.LogIfPossible(logsService);
                tasksService.AddOrUpdate(taskConverter.Data);
                return true;
            }
            return false;
        }
    }
}
