using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.IRepository;
using Metrix_MartAPIs.Repositories.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Metrix_MartAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _categories;
        private readonly ILogger<Categories> _logger;
        public CategoriesController(ILogger<Categories> logger, ICategoriesRepository categories)
        {
            _categories = categories;
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
        //{
        //    var categories = await _categories.GetAll().ToListAsync();
        //    return Ok(categories);
        //}

        [HttpPost("/addCategory")]
        public async Task<IActionResult> AddAsync(Categories categories)
        {
            try
            {
                _logger.LogInformation("Start Service Add Category: {DT}", DateTime.Now.ToLongTimeString());
                await _categories.AddAsync(categories);
                if(categories == null)
                {
                    _logger.LogInformation("Can't add Category: {DT}", DateTime.Now.ToLongTimeString());
                    return BadRequest("Adding Failed!");
                }
                _logger.LogInformation($"Add Category : {categories}", JsonConvert.SerializeObject(categories));
                return Ok(categories);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, "An Error Occurred while trying to add Category! {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpGet("/getAllCategories")]
        public IActionResult GetAllCategories()
        {
            try
            {
                _logger.LogInformation("Start Service >>> Get All Categories : {DT}", DateTime.Now.ToLongTimeString());
                var cates = _categories.GetAll();
                if (!cates.Any())
                {
                    _logger.LogInformation("Failed to get all Categories! {DT}", DateTime.Now.ToLongTimeString());
                    return NotFound("Categories Not Found1");
                }
                _logger.LogInformation($"Get all Categores : {cates}");
                return Ok(cates);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "An error occurred!");
                return StatusCode(500, "Internale Server Error");
            }
        }

        [HttpGet("/getCategoryName")]
        public async Task<IActionResult> GetCategoryName (string name)
        {
            try
            {
                _logger.LogInformation("Start Service >>> Get Category Name: {DT}", DateTime.Now.ToLongTimeString());
                var cateName = _categories.GetCategoriesName(name);
                if(cateName != null)
                {
                    _logger.LogInformation($"Category : {cateName}");
                }
                else
                {
                    _logger.LogInformation("Category not found! {DT}", DateTime.Now.ToLongTimeString());
                    return NotFound("Category is null!");
                }
                return Ok(cateName);
            }
            catch(Exception ex)
            {
                _logger.LogInformation("An Error Occurred while trying to get category's name! {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpPost("/updateCategory{id}")]
        public async Task<IActionResult> UpdateCategory(Categories categories, int id)
        {
            _logger.LogInformation("Start Service >>> Update Category {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var categ = JsonConvert.SerializeObject(categories);
                _logger.LogInformation($"Update Category {categ}");
                var cate = await _categories.UpdateCategory(categories, id);
                return Ok(cate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpDelete("/deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            _logger.LogInformation("Start Service >>> Delete Category {DT}", DateTime.Now.ToLongTimeString());

            try
            {
                // Await the task to get the IQueryable<Categories> 
                var categories = await _categories.Category;

                // Now you can use FirstOrDefaultAsync on the IQueryable
                var category = await categories.FirstOrDefaultAsync(c => c.CategoryId == id);

                if (category == null)
                {
                    _logger.LogError($"Category Not Found with Id: {id}");
                    return NotFound("Category not Found!");
                }
                return Ok("Category is Deleted!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred! {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Erorr!");
            }
        }
    }
}
