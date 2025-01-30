using Metrix_MartAPIs.Model;
using Metrix_MartAPIs.Repositories.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Metrix_MartAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<Product> _logger;
        public ProductController(IProductRepository product, ILogger<Product> logger)
        {
            _productRepository = product;
            _logger = logger;
        }

        [HttpPost("/addProduct")]
        public async Task<IActionResult> AddAsync(Product product)
        {
            _logger.LogInformation("Start Service >>> Add Product! {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                await _productRepository.AddAsync(product);
                if (product != null)
                {
                    _logger.LogInformation($"Product {product}", JsonConvert.SerializeObject(product));
                    return Ok(product);
                }
                else
                {
                    _logger.LogError("Ivalid or some fields are null! {DT}", DateTime.Now.ToLongTimeString());
                    return BadRequest("BadRequest!");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("An Error Occurred while trying to add product! {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpGet("/getProductById{id}")]
        public async Task<IActionResult> GetProductById (string productId)
        {
            _logger.LogInformation("Start Service >>> Get Product By Id {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var product = await _productRepository.GetProductById(productId);
                if (product == null)
                {
                    return NotFound("Product Id Not Found!");
                }
                return Ok(product);
            }
            catch(Exception ex)
            {
                _logger.LogError("An Error Occurred! {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpGet("/getProductName")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            _logger.LogInformation("Start Service >>> Geet Product by Name : {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var prdName = await _productRepository.GetProductByName(productName);
                if(prdName == null)
                {
                    _logger.LogInformation("Product Name not found! {DT}", DateTime.Now.ToLongTimeString());
                    return NotFound("Not found Product Name!");
                }
                _logger.LogInformation("End of Service >>> Get Product Name. {DT}", DateTime.Now.ToLongTimeString());
                return Ok(prdName);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred! {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("/getallProduct")]
        public IActionResult GetAllProducts()
        {
            _logger.LogInformation("Start Service >>> Get All Products : {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                var allProducts = _productRepository.GetAll();
                if (allProducts.Any())
                {
                    _logger.LogInformation($"Get All Products : {allProducts}", JsonConvert.SerializeObject(allProducts));
                    return Ok(allProducts);
                }
                else
                {
                    _logger.LogInformation("There are no Products found! {DT}", DateTime.Now.ToLongTimeString());
                    return NotFound("No Products are Found!");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred! {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpDelete("/deletebyId{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            _logger.LogInformation("Start Service >>> Delete Product by Id : {DT}", DateTime.Now.ToLongTimeString());
            try
            {
                //var product =await _productRepository.Product;
                var deleteProduct = await _productRepository.GetProductById(id);
                if(deleteProduct == null)
                {
                    _logger.LogInformation($"Product Id : {id} not found!");
                    return NotFound("Not found any product!");
                }
                else if(deleteProduct != null)
                {
                    return Ok("Product is Deleted!");
                }
                else
                {
                    return StatusCode(500, "Internale Server Error!");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Error Occurred! {DT}", DateTime.Now.ToLongTimeString());
                return StatusCode(500, "Internal Server Erorr!");
            }
        }
    }
}
