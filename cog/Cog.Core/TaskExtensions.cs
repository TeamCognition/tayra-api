using System;
using System.Threading.Tasks;

namespace Cog.Core
{
    /// <summary>
    ///     DOESN'T WORK ???
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        ///     Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known
        ///     as "Fire And Forget".
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="onException">
        ///     If an exception is thrown in the Task, <c>onException</c> will execute. If onException is
        ///     null, the exception will be re-thrown
        /// </param>
        public static void Forget(this Task task, Action<Exception> onException = null)
        {
            // note: this code is inspired by a tweet from Ben Adams. If someone find the link to the tweet I'll be pleased to add it here.
            // Only care about tasks that may fault (not completed) or are faulted,
            // so fast-path for SuccessfullyCompleted and Canceled tasks.
            if (!task.IsCompleted || task.IsFaulted)
                // use "_" (Discard operation) to remove the warning IDE0058: Because this call is not awaited, execution of the current method continues before the call is completed
                // https://docs.microsoft.com/en-us/dotnet/csharp/discards#a-standalone-discard
                _ = ForgetAwaited(task);
        }

        //TODO: turn this into a local function for above function on C# 8
        // Allocate the async/await state machine only when needed for performance reason.
        // More info about the state machine: https://blogs.msdn.microsoft.com/seteplia/2017/11/30/dissecting-the-async-methods-in-c/
        private static async Task ForgetAwaited(Task task, Action<Exception> onException = null)
        {
            try
            {
                // No need to resume on the original SynchronizationContext, so use ConfigureAwait(false)
                await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}