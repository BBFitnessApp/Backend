using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return Ok(products);
        }

        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        /*
        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            try
            {
                var _httpClient = new HttpClient();

                // Make request to Open Food Facts API
                HttpResponseMessage response = await _httpClient.GetAsync($"https://world.openfoodfacts.net/api/v2/product/{barcode}");

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error response from Open Food Facts API
                    return StatusCode((int)response.StatusCode);
                }

                // Deserialize Open Food Facts API response
                var contentStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Handle case-insensitive property matching
                };
                var openFoodFactsData = await JsonSerializer.DeserializeAsync<OpenFoodFactsProductResponse>(contentStream, options);

                // Map Open Food Facts data to your Product entity
                if (openFoodFactsData == null || openFoodFactsData.Nutriments == null)
                {
                    // Handle the case where the Nutriments data is missing
                    return NotFound();
                }

                var product = new Product
                {
                    Barcode = barcode,
                    Produktname = openFoodFactsData.Nutriments.Name!, // Use appropriate property name from the API response
                    Kalorien = Convert.ToInt32(openFoodFactsData.Nutriments.Kalorien), // Use appropriate property name from the API response
                    Fette = Convert.ToInt32(openFoodFactsData.Nutriments.Fette), // Use appropriate property name from the API response
                    Proteine = Convert.ToInt32(openFoodFactsData.Nutriments.Proteine), // Use appropriate property name from the API response
                    Kohlenhydrate = Convert.ToInt32(openFoodFactsData.Nutriments.Kohlenhydrate) // Use appropriate property name from the API response
                    // Map other relevant fields as needed
                };

                // Return the product to the frontend
                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }*/

        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet("barcode/json/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            try
            {
                var httpClient = new HttpClient();

                // Make request to Open Food Facts API
                HttpResponseMessage response = await httpClient.GetAsync($"https://world.openfoodfacts.net/api/v2/product/{barcode}");

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error response from Open Food Facts API
                    return StatusCode((int)response.StatusCode);
                }

                // Get the content of the response
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into an object
                dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse)!;

                // Extract relevant properties and map them to Product
                var product = new Product
                {
                    Barcode = barcode,
                    Produktname = jsonObject.product.product_name ?? string.Empty,
                    Kalorien = Convert.ToDouble(jsonObject.product?.nutriments?.energy_value ?? 0),
                    Fette = Convert.ToDouble(jsonObject.product?.nutriments?.fat ?? 0),
                    Proteine = Convert.ToDouble(jsonObject.product?.nutriments?.proteins ?? 0),
                    Kohlenhydrate = Convert.ToDouble(jsonObject.product?.nutriments?.carbohydrates ?? 0)
                    // Map other relevant fields as needed
                };

                // Return the product
                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
                [ProducesResponseType(typeof(Product), 201)]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [ProducesResponseType(204)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        [ProducesResponseType(204)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}
