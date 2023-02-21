using System.Collections.Generic;

namespace CottageApi.Core.Exceptions
{
    public class ValidationException : BusinessException
    {
        public ValidationException(string fieldName, string message)
            : base(message)
        {
            Errors = new List<ValidationError> { new ValidationError(fieldName, message) };
        }

        public ValidationException(ValidationError validationError)
            : base(validationError.Message)
        {
            Errors = new List<ValidationError> { validationError };
        }

        public List<ValidationError> Errors { get; }
    }
}