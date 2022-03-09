using Moq;
using Xunit;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;
using System;

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
                new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductId = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductId = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductId = 5, Name = "P5", Category = "Cat3" },
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
        public void Can_Send_PaginationViewModel()
        {
            var result = (_controller.Index(category: null, productPage: 2) as ViewResult)
                .ViewData
                .Model as ProductsListViewModel;

            var pageInfo = result.PagingInfo;

            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Use_Repository()
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

            var result = (controller.Index(category: null) as ViewResult)
                .ViewData
                .Model as ProductsListViewModel;

            var productsArray = result.Products.ToArray();

            Assert.True(productsArray.Length == 2);
            Assert.Equal("P1", productsArray[0].Name);
            Assert.Equal("P2", productsArray[1].Name);
        }

        [Fact]
        public void Can_Paginate()
        {
            var result = (_controller.Index(category: null, productPage: 2) as ViewResult)
                .ViewData
                .Model as ProductsListViewModel;

            var productsArray = result.Products.ToArray();

            Assert.True(productsArray.Length == 2);
            Assert.Equal("P4", productsArray[0].Name);
            Assert.Equal("P5", productsArray[1].Name);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            _controller.PageSize = 3;

            var result = (_controller.Index(category: "Cat2", productPage: 1) as ViewResult)
                .ViewData
                .Model as ProductsListViewModel;

            var productsArray = result.Products.ToArray();

            Assert.True(productsArray.Length == 2);
            Assert.True(productsArray[0].Name == "P2" && productsArray[0].Category == "Cat2");
            Assert.True(productsArray[1].Name == "P4" && productsArray[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Func<ViewResult, ProductsListViewModel> GetModel
                = result => result?.ViewData?.Model as ProductsListViewModel;

            _controller.PageSize = 3;

            var res1 = GetModel(_controller.Index("Cat1"))?.PagingInfo.TotalItems;
            var res2 = GetModel(_controller.Index("Cat2"))?.PagingInfo.TotalItems;
            var res3 = GetModel(_controller.Index("Cat3"))?.PagingInfo.TotalItems;
            var resAll = GetModel(_controller.Index(null))?.PagingInfo.TotalItems;

            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
