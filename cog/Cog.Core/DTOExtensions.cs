using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Cog.Core
{
    /// <summary>
    /// Defines extension methods for DTO class.
    /// </summary>
    public static class DTOExtensions
    {
        /// <summary>
        /// Adds property error for selected property and provided error message.
        /// </summary>
        /// <typeparam name="TSource">Object instance type.</typeparam>
        /// <typeparam name="TKey">Property type.</typeparam>
        /// <param name="source">Instance of object that inherits from DTO class.</param>
        /// <param name="keySelector">Property selector.</param>
        /// <param name="errorMessage">Validation error message.</param>
        /// <returns>Instance of property validation error.</returns>
        public static PropertyValidationError AddPropertyError<TSource, TKey>(this TSource source, Expression<Func<TSource, TKey>> keySelector, string errorMessage) where TSource : DTO
        {
            PropertyValidationError propertyValidationError = new PropertyValidationError
            {
                PropertyName = keySelector.GetPropertyPath(),
                ErrorMessage = errorMessage
            };
            source.PropertyValidationErrors.Add(propertyValidationError);
            return propertyValidationError;
        }

        /// <summary>
        /// Checks source object for property errors and throws PropertyValidationException if any are found.
        /// </summary>
        /// <typeparam name="TSource">Object instance type.</typeparam>
        /// <param name="source">Instance of object that inherits from DTO class.</param>
        public static void Validate<TSource>(this TSource source) where TSource : DTO
        {
            if (source.PropertyValidationErrors.Any())
            {
                throw new PropertyValidationException(source.PropertyValidationErrors);
            }
        }
    }
}
