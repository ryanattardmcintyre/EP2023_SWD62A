using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private ProductsRepository _productsRepository;
        public ProductsController(ProductsRepository productsRepository) { 
        _productsRepository= productsRepository;
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
         
    }
}
