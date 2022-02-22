using System.Linq;

namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly StoreDbContext context;

        public IQueryable<Product> Products => context.Products;

        public EFStoreRepository(StoreDbContext dbContext)
        {
            context = dbContext;
        }
    }
}
