using DataAccess.Repositories;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Validators
{
    public class ProductIdValidationAttribute: ValidationAttribute
    {
        public string GetErrorMessage() =>
          $"Product does not exist";

        //1st parameter....that gives you the value input by the user
        //2nd parameter....that gives you other useful stuff which can be used to validate the value 
        //e.g. the objects that were injected within the controller that received the input
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            //  var categoryInputByTheUser = (CreateProductViewModel)validationContext.ObjectInstance;
            var productId = (Guid)value;


            var productsWhateverRepository = (IProducts)validationContext.GetService(typeof(IProducts));


            var found = productsWhateverRepository.GetProduct(productId) != null;

            if (found == false)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
