using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CategoriesRepository
    {
        private ShoppingCartContext _shoppingCartContext;

        //constructor
        public CategoriesRepository(ShoppingCartContext shoppingCartContext)
        {
            _shoppingCartContext = shoppingCartContext;
        }

        public IQueryable<Category> GetCategories() {
            return _shoppingCartContext.Categories;
        }
    }
}
