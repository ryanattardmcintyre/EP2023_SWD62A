using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{

    //note: an interface contains only method signatures (no implementation)
    //note: an interface is like a contract
    public interface IProducts
    {
        IQueryable<Product> GetProducts();
        Product? GetProduct(Guid id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Guid id);
    }
}
