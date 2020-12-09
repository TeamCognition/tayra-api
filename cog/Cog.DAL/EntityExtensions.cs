namespace Cog.DAL
{
    public static class EntityExtensions
    {
        /// <summary>
        ///     Validates if object is null =&gt; throws EntityNotFoundException.
        /// </summary>
        /// <typeparam name="T">Type of object to validate.</typeparam>
        /// <param name="value">Object instance to validate.</param>
        /// <param name="identifier">Identifier for object instance.</param>
        /// <returns>Object instance if no exception is thrown.</returns>
        public static T EnsureNotNull<T>(this T value, object identifier) where T : class
        {
            if (value == null) throw new EntityNotFoundException<T>(identifier);
            return value;
        }

        /// <summary>
        ///     Validates if object is null =&gt; throws EntityNotFoundException.
        /// </summary>
        /// <typeparam name="T">Type of object to validate.</typeparam>
        /// <param name="TValue">Object instance to validate.</param>
        /// <param name="identifiers">Array of identifiers for object instance.</param>
        /// <returns>Object instance if no exception is thrown.</returns>
        public static T EnsureNotNull<T>(this T TValue, params object[] identifiers) where T : class
        {
            return TValue.EnsureNotNull(string.Join(":", identifiers));
        }
    }
}