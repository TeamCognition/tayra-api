using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cog.Core
{
    /// <summary>
    /// Base class to inherit from when creating DTO that must be validated.
    /// </summary>
    public abstract class DTO
    {
        /// <summary>
        /// List of property validation errors.
        /// </summary>
        [JsonIgnore]
        public List<PropertyValidationError> PropertyValidationErrors
        {
            get;
            set;
        }

        /// <summary>
        /// Base constructor.
        /// </summary>
        protected DTO()
        {
            PropertyValidationErrors = new List<PropertyValidationError>();
        }
    }
}
