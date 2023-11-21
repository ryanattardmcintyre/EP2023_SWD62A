using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private IProducts _productsRepository;
        private CategoriesRepository _categoriesRepository;
        public ProductsController(IProducts productsRepository, CategoriesRepository categoriesRepository) { 
            _productsRepository= productsRepository;
            _categoriesRepository= categoriesRepository;
        } 
        public IActionResult Index()
        {
            try
            {
                IQueryable<Product> list = _productsRepository.GetProducts().OrderBy(x => x.Name);

                var output =( from p in list
                             select new ListProductViewModel()
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Description = p.Description,
                                 Image = p.Image,
                                 Price = p.Price,
                                 Stock = p.Stock,
                                 CategoryName = _categoriesRepository.GetCategories().SingleOrDefault(x=>x.Id == p.CategoryFK).Name
                             }).ToList();


                return View(output);
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        //part 1: the method that loads the page with empty fields
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            CreateProductViewModel myModel = new CreateProductViewModel(_categoriesRepository);
            return View(myModel);
        }

        //part 2: the method which will receive the data typed by the user
        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateProductViewModel myModel, [FromServices] IWebHostEnvironment host)
        {
            //validation....must be done here
            string relativePath = "";
            try
            {
                ModelState.Remove("Categories"); //remove Categories from being checked for any validation issues

                if(ModelState.IsValid == false) //there was something wrong with one of the inputs and it is being signalled by a/some validator(s)
                {

                    myModel.Categories = _categoriesRepository.GetCategories();
                    return View(myModel);
                
                }    

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
                                             Image = relativePath,
                                             Owner = User.Identity.Name
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
                    Image = product.Image,
                    Owner = product.Owner
                };

                return View(model);
            }
        }
        public IActionResult Delete(Guid id)
        {
            try
            {
                if (User.Identity.Name == _productsRepository.GetProduct(id).Owner)
                {
 _productsRepository.DeleteProduct(id);
                TempData["message"] = "Product deleted successfully";
                }
                else
                {
                    TempData["error"] = "Access denied";
                }

               
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
            myModel.Id = id;

            return View(myModel);

        }

        [HttpPost]
        public IActionResult Edit(EditProductViewModel myModel, [FromServices] IWebHostEnvironment host)
        {
            //validation....must be done here
            string relativePath = "";
            try
            {

                if (ModelState.IsValid == false)
                {
                    myModel.Categories = _categoriesRepository.GetCategories();
                    return View(myModel);
                }


                if (_productsRepository.GetProduct(myModel.Id) == null) { 
                    TempData["error"] = "Product does not even exist";
                    return RedirectToAction("Index");
                    }


                if (myModel.ImageFile != null)
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

                    //delete the old image...so we don't waste any space for nothing
                    var oldImagePath = _productsRepository.GetProduct(myModel.Id).Image;
                    //to delete we need the absolute path
                    if (oldImagePath != null)
                    {
                        var absolutePathToDelete = host.WebRootPath + @"\images\" + System.IO.Path.GetFileName(oldImagePath);
                        //delete...
                        System.IO.File.Delete(absolutePathToDelete);
                    }

                }
                else
                {
                    //user would like to keep the old image
                    relativePath = _productsRepository.GetProduct(myModel.Id).Image;
                }

                //5. assign the relativepath to the Product() ...to be saved into the db

             
                    _productsRepository.UpdateProduct(new Product()
                    {
                        CategoryFK = myModel.CategoryFK,
                        Name = myModel.Name,
                        Description = myModel.Description,
                        Price = myModel.Price,
                        Stock = myModel.Stock,
                        Supplier = myModel.Supplier,
                        WholesalePrice = myModel.WholesalePrice,
                        Image = relativePath,
                        Id= myModel.Id //<<<<<<<<<<<< i need to pass the id received from the hidden field, so the UpdateProduct method knows which product to update
                    });

                    TempData["message"] = "Product updated successfully";
                
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                myModel.Categories = _categoriesRepository.GetCategories();
                TempData["error"] = "Product was not updated successfully";
                return View(myModel);
            }



        }

    }
}
