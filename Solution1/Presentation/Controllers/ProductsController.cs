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
        public IActionResult Create(CreateProductViewModel myModel, [FromServices] IWebHostEnvironment host)
        {
            //validation....must be done here
            string relativePath = "";
            try
            {
                //1. rename the image file with a unique name
                //eg. B269FE87-E6D3-4508-BAB0-1E0D0EE838C6.jpg
                string filename = Guid.NewGuid() + System.IO.Path.GetExtension(myModel.ImageFile.FileName);

                //2. absolute path - e.g. C:\Users\attar\Source\Repos\EP2023_SWD62A\Solution1\Presentation\wwwroot\images\<filename.jpg>
                //IWebHostEnvironment
                string absolutePath = host.WebRootPath + @"\images\" + filename;

                //3. relative path
                //    \t \r \n 
                relativePath = @"/images/" + filename;

                //4. save the image into the absolute path folder

                using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                {
                    myModel.ImageFile.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                }

                //5. assign the relativepath to the Product() ...to be saved into the db

                if (_productsRepository.GetProducts().Where(x=>x.Name == myModel.Name).Count() == 0)
                {
                         _productsRepository.AddProduct(new Product()
                                        {
                                            CategoryFK = myModel.CategoryFK,
                                            Name = myModel.Name,
                                            Description = myModel.Description,
                                            Price = myModel.Price,
                                            Stock = myModel.Stock,
                                            Supplier = myModel.Supplier,
                                            WholesalePrice = myModel.WholesalePrice,
                                             Image = relativePath
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

        //purpose: this is going to open a page with controls filled with the existent data
        [HttpGet]
        public IActionResult Edit(Guid id) {
            EditProductViewModel myModel = new EditProductViewModel(_categoriesRepository);
            

            //we are doing this because...
            //...we are fetching the current product details from db to show them to the end-user
            //...on screen so we assist the user while editing
            var originalProduct = _productsRepository.GetProduct(id);
           
            myModel.Supplier = originalProduct.Supplier;
            myModel.WholesalePrice = originalProduct.WholesalePrice;
            myModel.Price = originalProduct.Price;
            myModel.Name = originalProduct.Name;
            myModel.Description = originalProduct.Description;
            myModel.CategoryFK = originalProduct.CategoryFK;
            myModel.Image = originalProduct.Image;
            myModel.Stock = originalProduct.Stock;

            return View(myModel);

        }

      //  [HttpPost]
       // public IActionResult Edit(...) { }


    }
}
