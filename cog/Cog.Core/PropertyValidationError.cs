namespace Cog.Core
{
    /// <summary>
    /// DTO class to hold information about property validation error.
    /// </summary>
    public class PropertyValidationError
    {
        /// <summary>
        /// Name of the property where the validation error has occured.
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Validation error message.
        /// </summary>
        public string ErrorMessage
        {
            get;
            set;
        }
    }
}
