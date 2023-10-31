using DataAccess.DataContext;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        //constructor injection
        //private ProductsRepository _repo;
        //public AdminController(ProductsRepository repo) {

        //    _repo = repo;

        //}

        
        //method injection
        public IActionResult Index([FromServices]ShoppingCartContext context)
        {

            //property injection
          //   ProductsRepository productsRepository = new ProductsRepository();
         //   productsRepository._shoppingCartContext = context;

           
            //check if person is logged in
            //if person is logged in continue
            //else do not allow in - access denied / redirect the user to the login page

            if (User.Identity.IsAuthenticated == false) {


                TempData["error"] = "Access denied";


                // return View("Error", Request); //this will look for a View with a name "Error"
                return RedirectToAction("Index", "Home");   
            
            }


            return View();
        }

        //example of method injection

        //public IActionResult Details([FromServices] ProductsRepository _repo) {

        //    _repo.GetProduct(id);
        //}
    }
}
