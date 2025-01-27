using Metrix_MartAPIs.DbContexts;
using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;
using Metrix_MartAPIs.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Metrix_MartAPIs.Repositories.Repository
{
    public class CategoriesRepository : GenericRepository<Categories>, ICategoriesRepository
    {
        private readonly MetrixMartDbContext _context;
        private readonly ILogger _logger;

        

        public CategoriesRepository(ILogger<Categories> logger, MetrixMartDbContext context) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IQueryable<Categories>> Category()
        {
            _logger.LogInformation("Start Service >>> Get All Categories: {DT}", DateTime.Now.ToLongTimeString());

            try
            {
                var categories = await _context.Categories.ToListAsync(); // Use ToListAsync for async operations

                if (!categories.Any())
                {
                    _logger.LogInformation("No categories found.");
                }

                _logger.LogInformation($"Retrieved {categories.Count} categories.");
                return _context.Categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred while trying to Get All Categories! {DT}", DateTime.Now.ToLongTimeString());
                throw; // Re-throw the exception for proper error handling
            }
        }

        public override async Task<Categories> AddAsync(Categories category)
        {
            _logger.LogInformation("Start Service Add Category : {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                
                int latestId = await _context.Categories.OrderByDescending(e => e.CategoryId)
                                                           .Select(e => e.CategoryId)
                                                           .FirstOrDefaultAsync();
                int newCategoryId = latestId + 1;

                //string newId = $"CAT{newCategoryId:000}";
                category.CategoryId = newCategoryId;
                await base.AddAsync(category);
                await _context.SaveChangesAsync();
                var cat = JsonConvert.SerializeObject(category);
                _logger.LogInformation($"Add Category successfully : {cat}");
                return category;
            }
            catch(Exception ex)
            {
                _logger.LogInformation("An Error Occurred while trying to add Category! {DT}", DateTime.Now.ToLongTimeString());
                throw new Exception("Addding Failed", ex); 
            }
        }

        public async Task<bool> DeleteCategoryById(int id)
        {
            try
            {
                _logger.LogInformation("Start Service >>> Delete Category by Id {DT}", DateTime.Now.ToLongTimeString());
                var categ = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
                if (categ == null)
                {
                    _logger.LogError("Category not found! {DT}", DateTime.Now.ToLongTimeString());
                }
                _logger.LogInformation($"Delete Category : {categ}");
                _context.Categories.Remove(categ);
                await _context.SaveChangesAsync();
                _logger.LogInformation("End of Service >>> Delete Category : {DT}", DateTime.Now.ToLongTimeString());
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred while trying to Delete Category : {DT}", DateTime.Now.ToLongTimeString());
                return false;
            }
        }

        public List<Categories> GetAll()
        {
            try
            {
                _logger.LogInformation("Start Service >>> Get All Categories: {DT}", DateTime.Now.ToLongTimeString());
                var cate = _context.Categories.ToList();
                if (!cate.Any())
                {
                    _logger.LogInformation("Can't Get All Categorues {DT}", DateTime.Now.ToLongTimeString());
                    return new List<Categories>();
                }
                var categories = JsonConvert.SerializeObject(cate);
                _logger.LogInformation($"All Categories : {categories}");
                return cate;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An Error Occurred while trying to Get All Categories! {DT}", DateTime.Now.ToLongTimeString());
                return new List<Categories>();
            }
        }

        public Categories GetCategoriesName(string name)
        {
            _logger.LogInformation("Start Service >>> Get Categories By Name :{DT}", DateTime.Now.ToLongTimeString());
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogInformation("Category's name is null or empty. {DT}", DateTime.Now.ToLongTimeString());
                    return null;
                }
                var categoryName = _context.Categories
                                           .Where(c => c.CategoryName.ToLower() == name.ToLower())
                                           .FirstOrDefault();
                if (name != null)
                {
                    var cateName = JsonConvert.SerializeObject(categoryName);
                    _logger.LogInformation($"Category : {cateName}");
                }
                else
                {
                    _logger.LogInformation("Category Name not found! {DT}", DateTime.Now.ToLongTimeString());
                }
                return categoryName;
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, "An error occurred! {DT}", DateTime.Now.ToLongTimeString());
                throw; 
            }
        }


        public async Task<Categories> UpdateCategory(Categories category, int id)
        {
            _logger.LogInformation("Start Service >>> Update Category {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var categ = JsonConvert.SerializeObject(category);
                _logger.LogInformation($"Update Category : {categ}");
                _context.Categories.Update(category);
                _context.SaveChanges();
                _logger.LogInformation("End of Service >>> Update Category :{DT}", DateTime.Now.ToLongTimeString());
                return category;
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "An Error Occurred! >>>> UpdateDbException. {DT}", DateTime.Now.ToLongTimeString());
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred! {DT}", DateTime.Now.ToLongTimeString());
                throw;
            }
        }
    }
}
