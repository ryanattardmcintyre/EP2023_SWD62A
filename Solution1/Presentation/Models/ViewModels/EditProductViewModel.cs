using DataAccess.Repositories;
using Domain.Models;
using Presentation.Validators;
using System.ComponentModel;

namespace Presentation.Models.ViewModels
{
    public class EditProductViewModel
    {
        public EditProductViewModel() { }
        public EditProductViewModel(CategoriesRepository categoriesRepository)
        {

            Categories = categoriesRepository.GetCategories();   //populate the list of Categories
        }

        [ProductIdValidation]
        public Guid Id { get; set; } //is needed because we need to know which product is to be edited!

        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public string? Image { get; set; }

        public IFormFile ImageFile { get; set; }
        public IQueryable<Category> Categories { get; set; }
        
        [CategoryValidation]
        public int CategoryFK { get; set; } //foreign key property

        [DisplayName("Wholesale Price")]
        public double WholesalePrice { get; set; }
        public string? Supplier { get; set; }
    }
}
