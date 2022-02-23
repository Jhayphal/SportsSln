using Moq;
using System;
using Xunit;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IStoreRepository> _repositoryMock;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            var products = new Product[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" },
            };

            _repositoryMock = new Mock<IStoreRepository>();

            _repositoryMock
                .Setup(m => m.Products)
                .Returns(products.AsQueryable());

            _controller = new HomeController(_repositoryMock.Object)
            {
                PageSize = 3
            };
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            var result = (_controller.Index(productPage: 2) as ViewResult)
                .ViewData
                .Model as ProductsListViewModel;

            var pageInfo = result.PagingInfo;

            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

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
                .Model as ProductsListViewModel;

            var productsArray = result.Products.ToArray();

            Assert.True(productsArray.Length == 2);
            Assert.Equal("P1", productsArray[0].Name);
            Assert.Equal("P2", productsArray[1].Name);
        }

        [Fact]
        public void CanPaginate()
        {
            var result = (_controller.Index(productPage: 2) as ViewResult)
                .ViewData
                .Model as ProductsListViewModel;

            var productsArray = result.Products.ToArray();

            Assert.True(productsArray.Length == 2);
            Assert.Equal("P4", productsArray[0].Name);
            Assert.Equal("P5", productsArray[1].Name);
        }
    }
}
