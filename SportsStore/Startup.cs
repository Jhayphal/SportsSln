using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Microsoft.AspNetCore.Http;

namespace SportsStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<StoreDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:SportsStoreConnection"]);
            });
            services.AddScoped<IStoreRepository, EFStoreRepository>();
            services.AddRazorPages();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "catpage",
                    "{category}/Page{productPage:int}",
                    new
                    {
                        Controller = "Home",
                        action = "Index"
                    });

                endpoints.MapControllerRoute(
                    "page",
                    "Page{productPage:int}",
                    new
                    {
                        Controller = "Home",
                        action = "Index",
                        productPage = 1
                    });

                endpoints.MapControllerRoute(
                    "category",
                    "{category}",
                    new
                    {
                        Controller = "Home",
                        action = "Index",
                        productPage = 1
                    });

                endpoints.MapControllerRoute(
                    "pagination",
                    "Products/Page{productPage}",
                    new 
                    { 
                        Controller = "Home", 
                        action = "Index",
                        productPage = 1
                    });

                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

            SeedData.EnsurePopulated(app);
        }
    }
}
