using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloakedDagger.Common.Exceptions;


namespace CloakedDagger.Logic
{
    public static class ValidationUtils
    {
        public static void ValidateViewModel(object viewModel, string message = "")
        {
            var validationContext = new ValidationContext(viewModel, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, validationContext, results, true);

            if (!isValid)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new EntityValidationException(results);
                }
                
                throw new EntityValidationException(message, results);
            }
        }
    }
}