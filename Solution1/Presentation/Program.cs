using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
 

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ShoppingCartContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ShoppingCartContext>();
            builder.Services.AddControllersWithViews();


            //these lines they basically register the type of class with the services collection (so that the injector class
            //is aware of which classes have to be initialized and eventually requested by client classes)
            //builder.Services.AddScoped(typeof(ProductsRepository) );

            //need the path to the Data Folder //C:\Users\attar\source\repos\EP2023_SWD62A\Solution1\Presentation \Data\

            string pathToJsonFile = builder.Environment.ContentRootPath + "Data\\products.json";
            builder.Services.AddScoped(typeof(CategoriesRepository));

            
            builder.Services.AddScoped<IProducts, ProductsRepository>();
            //builder.Services.AddScoped<IProducts, ProductsJsonRepository>(x => new ProductsJsonRepository(pathToJsonFile));



            //we have 3 methods which one can use to determine/control how many instances of the chosen classes are actually created

            //AddScoped - will create ONE/an instance one for every request
            //AddTransient - will create an x no. of instances depending on how many calls you have
            //AddSingleton - will create ONE instance for all calls, for all requests AND for all users


            var app = builder.Build();

          

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}