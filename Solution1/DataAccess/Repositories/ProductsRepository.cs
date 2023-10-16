﻿using DataAccess.DataContext;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //Dependency Injection - design pattern that manages the creation of instances

    public class ProductsRepository
    {

        private ShoppingCartContext _shoppingCartContext;

        //constructor
        public ProductsRepository(ShoppingCartContext shoppingCartContext ) {
            _shoppingCartContext= shoppingCartContext;  
        }

        //What is the difference between IQueryable and List
        //1. (drawback) in IQueryable you cannot debug it while in the List you can
        //2. (advantage) IQueryable never opens a call to the database until you convert it into a List

        /* Example:
         * 
         * var myCommand = GetProducts().Where(x=>x.Name.Contains(variable).Skip(10).Take(10).OrderBy(x=>x.Name);
         * myCommand.ToList();// <<<<
         */
        //methods
        public IQueryable<Product> GetProducts()
        {
           return _shoppingCartContext.Products; 
            //the list of products is not retrieved not until you call ToList()
        }

        public Product? GetProduct(Guid id) {
          
            return _shoppingCartContext.Products.SingleOrDefault(x => x.Id==id);

        }

        public void AddProduct(Product product) {

            sh.Add(product);
        }

    }
}
