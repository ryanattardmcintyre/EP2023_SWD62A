using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
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
    }
}
