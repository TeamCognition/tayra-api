using Cog.Core;

namespace Cog.DAL
{
    /// <summary>
    /// This exception should be thrown when expected entity for transaction is not found when it should.
    /// This is not managed exception and presents either conflict or security breach attempt.
    /// Do not throw this exception when performing regular search queries.
    /// </summary>
    /// <example>
    /// Doing something as /Delete/House/3 when there is no House object with Id 3 in the database =&gt; conflict.
    /// Doing something as /View/Article/43 when user is not allowed to see articles from other users =&gt; securith breach.
    /// </example>
    /// <typeparam name="T">Type of entity exception is related to.</typeparam>
    public class EntityNotFoundException<T> : CogSecurityException where T : class
    {
        public EntityNotFoundException(object identifier)
            : base(GetMessage(identifier), ".ctor")
        {
        }

        private static string GetMessage(object identifier)
        {
            return $"Entity type '{typeof(T)}' was not found for identifier '{identifier}'";
        }
    }
}
