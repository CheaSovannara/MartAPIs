using Metrix_MartAPIs.DbContexts;
using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.GenericRepository;
using Metrix_MartAPIs.Repositories.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Metrix_MartAPIs.Repositories.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly MetrixMartDbContext _context;
        private readonly ILogger _logger;

        public ProductRepository(ILogger<Product> logger, MetrixMartDbContext context) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IQueryable<Product>> Product()
        {
            _logger.LogInformation("Start Service >>> Query Products : {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var products = await _context.Products.ToListAsync();
                if (!products.Any())
                {
                    _logger.LogError($"No Products are Found! {products}", JsonConvert.SerializeObject(products));
                }
                _logger.LogInformation($"Retreived {products.Count} products.");
                return _context.Products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred while trying to Get All Categories! {DT}", DateTime.Now.ToLongTimeString());
                throw; // Re-throw the exception for proper error handling
            }
        }

        public override async Task<Product> AddAsync(Product product)
        {
            _logger.LogInformation("Start Service >>> Add a Product {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                string lastId = await _context.Products.OrderByDescending(p => p.ProductId)
                                                       .Select(p => p.ProductId)
                                                       .FirstOrDefaultAsync();
                int latestId = 0;
                if (!string.IsNullOrWhiteSpace(lastId) && int.TryParse(lastId.Replace("PRD", ""), out latestId))
                {
                    latestId++;
                }


                //Generate Product Id
                string newId = $"PRD{latestId:000}";
                product.ProductId = newId;

                // Assuming CategoryId is the primary key of the Categories entity
                int categoryId = (int)product.CategoryId;
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    throw new ArgumentException("Invalid Category Id provided!");
                }
                await base.AddAsync(product);
                await _context.SaveChangesAsync();
                var products = JsonConvert.SerializeObject(product);
                _logger.LogInformation($"Added Product : {products}");
                return product;
            }
            catch(Exception ex)
            {
                _logger.LogError("An Error Occurred while trying to add Product! {DT}", DateTime.Now.ToLongTimeString());
                throw ex;
            }
        }

        public async Task<bool> DeleteById(string id)
        {
            _logger.LogInformation("Start Service >>> Delete Product : {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var prd = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                
                if(prd == null)
                {
                    _logger.LogError("Product not found! {DT}", DateTime.Now.ToLongTimeString());
                    return false;
                }
                else if(prd.CategoryId != null)
                {
                    var depenId = await _context.Products.AnyAsync(p => p.CategoryId == prd.CategoryId);
                    if (depenId)
                    {
                        _logger.LogInformation($"Product with Id {id} has Dependent entities will Delete.");
                        return true;
                    }
                    _logger.LogInformation("End of Service delete Product: {DT}", DateTime.Now.ToLongTimeString());
                    _context.Products.Remove(prd);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Handle potential foreign key constraint violations
                if (ex is DbUpdateException && ex.InnerException is SqlException sqlException &&
                    sqlException.Number == 547) // Check for specific foreign key constraint violation error number
                {
                    _logger.LogError($"Cannot delete product. Foreign key constraint violation: {sqlException.Message}");
                    return false;
                }

                // Log other exceptions
                _logger.LogError(ex, "An Error Occurred! {DT}", DateTime.Now.ToLongTimeString());
                return false;
            }
        }

        public List<Product> GetAll()
        {
            _logger.LogInformation("Start Service >>> Get All Products {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var allProducts = _context.Products.ToList();
                if (allProducts.Any())
                {
                    var products = JsonConvert.SerializeObject(allProducts);
                    _logger.LogInformation($"Get Products : {products}");
                }
                _logger.LogInformation("End of Service >>> Get all products: {DT}", DateTime.Now.ToLongTimeString());
                return allProducts;
            }
            catch(Exception ex)
            {
                _logger.LogError("an error occurred while trying to get all products {DT}", DateTime.Now.ToLongTimeString());
                return new List<Product>();
            }
        }


        public async Task<Product> GetProductByName(string productName)
        {
            _logger.LogInformation("Start Service >>> Get Product By Name {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                _context.Database.SetCommandTimeout(60); // Set timeout to 60 seconds (or appropriate value)

                var prdName = await _context.Products.Where(p => p.ProductName.ToLower() == productName.ToLower()).FirstOrDefaultAsync();

                if (productName == null)
                {
                    _logger.LogError($"Product Name not found {productName}");
                    return null;
                }
                var prdNaming = JsonConvert.SerializeObject(prdName);
                _logger.LogInformation($"Product by Name is : {prdNaming}");
                return prdName;
            }
            catch (TaskCanceledException ex)
            {
                // Log the cancellation exception with more context
                _logger.LogError($"Task was canceled while fetching product: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Log general exceptions with full details
                _logger.LogError($"An error occurred: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task<Product> GetProductById(string productId)
        {
            _logger.LogInformation("Start Service >>> Get Product by Id : {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
                if(product == null)
                {
                    _logger.LogError("Product not found! {DT}", DateTime.Now.ToLongTimeString());
                    return null;
                }
                var prId = JsonConvert.SerializeObject(product);
                _logger.LogInformation($"Search Product by Id : {prId}");
                return product;
            }
            catch(Exception ex)
            {
                _logger.LogError("An Error Occurred while trying to find ProductId {DT}", DateTime.Now.ToLongTimeString());
                throw; 
            }
        }

        public void Update(Product tentiy)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product), "Product cannot be null.");
                }

                // Check if the product exists in the database
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID '{product.ProductId}' not found.");
                    return false;
                }

                // Update the existing product with the new values
                existingProduct.ProductName = product.ProductName;
                existingProduct.ProductPrice = product.ProductPrice;
                // Update other properties as needed

                _context.Entry(existingProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product.");
                return false;
            }
        }
    }
}
