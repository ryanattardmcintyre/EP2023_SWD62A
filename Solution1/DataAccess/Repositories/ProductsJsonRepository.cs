using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace DataAccess.Repositories
{
    public class ProductsJsonRepository : IProducts
    {

        private string _path;
        public ProductsJsonRepository(string path ) { 
        _path = path;

            if (File.Exists(_path) == false)
            {
                using (FileStream fs = File.Create(path))
                {
                    fs.Close();
                }
            }
        }

        public void AddProduct(Product product)
        {
            product.Id = Guid.NewGuid();

            var list = GetProducts().ToList();
            list.Add(product);

            var allProductsText = JsonSerializer.Serialize(list);

            try
            {
                File.WriteAllText(_path, allProductsText);
            }
            catch (Exception ex)
            {
                //log
                throw new Exception("Error while adding product");

            }
        }

        public void DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Product? GetProduct(Guid id)
        {
           return GetProducts().SingleOrDefault(p => p.Id == id);
        }

        public IQueryable<Product> GetProducts()
        {
            if (File.Exists(_path)) //checking for file existence
            {

                try
                {
                    string allText = "";
                    //read data from the file
                    using (StreamReader sr = File.OpenText(_path))
                    {
                        allText = sr.ReadToEnd();
                        sr.Close();
                    }

                    if (string.IsNullOrEmpty(allText))
                    {
                        return new List<Product>().AsQueryable();
                    }

                    List<Product> myProducts = JsonSerializer.Deserialize<List<Product>>(allText); //converts from jsonstring into an object

                    return myProducts.AsQueryable();
                }
                catch(Exception ex)
                {
                    //log...
                    throw new Exception("Error while opening the file");
                }
            }
            else throw new Exception("File saving products not found");
        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
