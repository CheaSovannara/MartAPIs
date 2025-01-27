using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;

namespace Metrix_MartAPIs.Repositories.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<Product> GetProductById(string productId);
        public Task<Product> GetProductByName(string productName);
        Task<bool> DeleteById (string id);
        Task<IQueryable<Product>> Product();
    }
}
