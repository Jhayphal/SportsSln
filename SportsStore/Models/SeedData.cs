using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace SportsStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            StoreDbContext context = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<StoreDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Products.Any())
            {
                var products = CreateDefaultProducts();
                context.Products.AddRange(products);

                context.SaveChanges();
            }
        }

        private static Product[] CreateDefaultProducts()
        {
            return new Product[]
            {
                new Product
                {
                    Name = "Kayak",
                    Description = "A boat for one person",
                    Category = "Watersports",
                    Price = 275
                },
                new Product
                {
                    Name = "Lifejacket",
                    Description = "Protective and feshionable",
                    Category = "Watersports",
                    Price = 48.95m
                },
                new Product
                {
                    Name = "Soccer Ball",
                    Description = "FIFA-Approwed size and weight",
                    Category = "Soccer",
                    Price = 19.50m
                },
                new Product
                {
                    Name = "Corner Flags",
                    Description = "Give your playing field a professional touch",
                    Category = "Soccer",
                    Price = 34.95m
                },
                new Product
                {
                    Name = "Stadium",
                    Description = "Flat-packed 35,000-seat stadium",
                    Category = "Soccer",
                    Price = 79500
                },
                new Product
                {
                    Name = "Thinking Cap",
                    Description = "Improve brain efficiency by 75%",
                    Category = "Chess",
                    Price = 16
                },
                new Product
                {
                    Name = "Unsteady Chair",
                    Description = "Secretly give your opponent disadvantage",
                    Category = "Chess",
                    Price = 29.95m
                },
                new Product
                {
                    Name = "Human Chess Board",
                    Description = "A fun game for the family",
                    Category = "Chess",
                    Price = 75
                },
                new Product
                {
                    Name = "Bling-bling King",
                    Description = "Gold-plated, diamond-studded King",
                    Category = "Chess",
                    Price = 1200
                }
            };
        }
    }
}
