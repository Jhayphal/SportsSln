using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        public int PageSize = 4;

        private readonly IStoreRepository repository;

        public HomeController(IStoreRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index(int productPage = 1) 
            => View
            (
                repository
                    .Products
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize)
            );
    }
}
