using DataAccess.Repositories;
using Domain.Models;
using Presentation.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class CreateProductViewModel
    {

        public CreateProductViewModel() { }
        public CreateProductViewModel(CategoriesRepository categoriesRepository) {

            Categories = categoriesRepository.GetCategories();   //populate the list of Categories
        }


        //Compare 
        //RegularExpression

        
        [Required(AllowEmptyStrings =false, ErrorMessage ="Please type something as a name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0,double.MaxValue, ErrorMessage ="Price input is out of range" )]
        public double Price { get; set; }

        public int Stock { get; set; }

        //public string? Image { get; set; }

        public IFormFile ImageFile { get; set; }
        public IQueryable<Category> Categories { get; set; }

        [CategoryValidation]
        public int CategoryFK { get; set; } //foreign key property //3

        [DisplayName("Wholesale Price")]
        public double WholesalePrice { get; set; }
        public string? Supplier { get; set; }


    }
}
