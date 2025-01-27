using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;

namespace Metrix_MartAPIs.Repositories.IRepository
{
    public interface ICategoriesRepository : IGenericRepository<Categories>
    {
        public Categories GetCategoriesName(string name);
        Task<bool> DeleteCategoryById(int id);
        Task<Categories> UpdateCategory(Categories categories, int id);
        Task<IQueryable<Categories>> Category();
    }
}
