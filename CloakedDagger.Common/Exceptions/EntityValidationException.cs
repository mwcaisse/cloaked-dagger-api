using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloakedDagger.Common.Exceptions
{
    public class EntityValidationException : Exception
    {
        private readonly IEnumerable<ValidationResult> _validationResults;

        public IEnumerable<ValidationResult> ValidationResults => _validationResults;

        public EntityValidationException(IEnumerable<ValidationResult> validationResults = null) : base("Validation issues exist.")
        {
            _validationResults = validationResults ?? new List<ValidationResult>();
        }

        public EntityValidationException(string message, IEnumerable<ValidationResult> validationResults = null)
            : base(message)
        {
            _validationResults = validationResults ?? new List<ValidationResult>();
        }

        public EntityValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            _validationResults = new List<ValidationResult>();
        }
        
    }
}