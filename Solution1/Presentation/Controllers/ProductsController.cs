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

                if(_productsRepository.GetProducts().Where(x=>x.Name == myModel.Name).Count() == 0)
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
                }

               

              

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                myModel.Categories = _categoriesRepository.GetCategories();
                TempData["error"] = "Product was not inserted successfully";
                return View(myModel);
            }


            
        }


        public IActionResult Details(Guid id)
        {
            var product = _productsRepository.GetProduct(id);
            if (product == null)
            {
                TempData["error"] = "Product was not found";
                return RedirectToAction("Index");

            }
            else
            {
                ListProductViewModel model = new ListProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    Description = product.Description,
                    CategoryName = product.Category.Name,
                    Image = product.Image
                };

                return View(model);
            }
        }


        public IActionResult Delete(Guid id)
        {
            try
            {
                _productsRepository.DeleteProduct(id);
                TempData["message"] = "Product deleted successfully";
             }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not deleted; Input might have been tampered and product not found";
            }

            return RedirectToAction("Index");
        }


    }
}
