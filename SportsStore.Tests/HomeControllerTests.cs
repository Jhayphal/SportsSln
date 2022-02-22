using Moq;
using System;
using Xunit;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SportsStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CanUseRepository()
        {
            var products = new Product[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" }
            };

            var mock = new Mock<IStoreRepository>();

            mock
                .Setup(m => m.Products)
                .Returns(products.AsQueryable());

            var controller = new HomeController(mock.Object);

            var result = (controller.Index() as ViewResult)
                .ViewData
                .Model as IEnumerable<Product>;

            var productsArray = result.ToArray();

            Assert.True(productsArray.Length == 2);
            Assert.Equal("P1", productsArray[0].Name);
            Assert.Equal("P2", productsArray[1].Name);
        }

        [Fact]
        public void CanPaginate()
        {
            var products = new Product[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" },
            };

            var mock = new Mock<IStoreRepository>();

            mock
                .Setup(m => m.Products)
                .Returns(products.AsQueryable());

            var controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            var result = (controller.Index(productPage: 2) as ViewResult)
                .ViewData
                .Model as IEnumerable<Product>;

            var productsArray = result.ToArray();

            Assert.True(productsArray.Length == 2);
            Assert.Equal("P4", productsArray[0].Name);
            Assert.Equal("P5", productsArray[1].Name);
        }
    }
}
