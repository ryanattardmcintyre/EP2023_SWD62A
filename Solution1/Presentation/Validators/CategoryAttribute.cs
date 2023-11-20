using DataAccess.Repositories;
using Humanizer.Localisation;
using Presentation.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Validators
{
    public class CategoryValidationAttribute : ValidationAttribute
    {
        public CategoryValidationAttribute()
        { }

        public string GetErrorMessage() =>
            $"Category input is not valid";

        //1st parameter....that gives you the value input by the user
        //2nd parameter....that gives you other useful stuff which can be used to validate the value 
                           //e.g. the objects that were injected within the controller that received the input
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
          //  var categoryInputByTheUser = (CreateProductViewModel)validationContext.ObjectInstance;
            var categoryInputByTheUser = (int)value;


            var categoriesRepository = (CategoriesRepository)validationContext.GetService(typeof(CategoriesRepository));


            //Select Min(Id) From Categories
            var minCategoryId = categoriesRepository.GetCategories().Min(x => x.Id);
            var maxCategoryId = categoriesRepository.GetCategories().Max(x => x.Id);
             
            if (categoryInputByTheUser <minCategoryId || categoryInputByTheUser > maxCategoryId)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
    


}
