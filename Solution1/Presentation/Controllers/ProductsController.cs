using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private ProductsRepository _productsRepository;
        private CategoriesRepository _categoriesRepository;
        public ProductsController(ProductsRepository productsRepository, CategoriesRepository categoriesRepository) { 
            _productsRepository= productsRepository;
            _categoriesRepository= categoriesRepository;
        } 
        public IActionResult Index()
        {
          IQueryable<Product> list = _productsRepository.GetProducts().OrderBy(x=>x.Name);

            var output = from p in list
                         select new ListProductViewModel()
                         {
                             Id = p.Id,
                             Name = p.Name,
                             Description = p.Description,
                             Image = p.Image,
                             Price = p.Price,
                             Stock = p.Stock,
                             CategoryName = p.Category.Name
                         };
          

          return View(output);
        }


        //part 1: the method that loads the page with empty fields
        [HttpGet]
        public IActionResult Create()
        {
            CreateProductViewModel myModel = new CreateProductViewModel(_categoriesRepository);
            return View(myModel);
        }

        //part 2: the method which will receive the data typed by the user
        [HttpPost]
        public IActionResult Create(CreateProductViewModel myModel)
        {
            //validation....must be done here
            try
            {
                _productsRepository.AddProduct(new Product()
                {
                    CategoryFK = myModel.CategoryFK,
                    Name = myModel.Name,
                    Description = myModel.Description,
                    Price = myModel.Price,
                    Stock = myModel.Stock,
                    Supplier = myModel.Supplier,
                    WholesalePrice = myModel.WholesalePrice
                });

                TempData["message"] = "Product saved successfully";

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not inserted successfully";
                return View(myModel);
            }


            
        }
    }
}
