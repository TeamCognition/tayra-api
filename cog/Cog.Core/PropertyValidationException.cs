using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cog.Core
{
    /// <summary>
    /// Throw this exception when you want to return property error as tooltip on UI.
    /// </summary>
    /// <example>
    /// E-mail address is in use, but query must be made from the repository before statement has been made.
    /// </example>
    public class PropertyValidationException : Exception
    {
        public IEnumerable<PropertyValidationError> PropertyValidationErrors
        {
            get;
            private set;
        }

        public PropertyValidationException()
        {
        }

        public PropertyValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public PropertyValidationException(string propertyName, string errorMessage)
        {
            PropertyValidationErrors = new List<PropertyValidationError>
            {
                new PropertyValidationError
                {
                    PropertyName = propertyName,
                    ErrorMessage = errorMessage
                }
            };
        }

        public PropertyValidationException(List<PropertyValidationError> propertyValidationErrors)
        {
            PropertyValidationErrors = propertyValidationErrors;
        }
    }
}
