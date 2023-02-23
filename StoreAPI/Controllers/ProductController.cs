using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;
using StoreAPI.Services;
using StoreAPI.Validators;

namespace StoreAPI.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly ProductService _productService;

    public ProductController(ILogger<ProductController> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet("products")]
    public async Task<List<Product>> Get()
    {
        return await _productService.GetAsync();
    }

    [HttpGet("sort")]
    // api/Product/list?orderBy=name
    // returns products as ordered by desired property
    public async Task<ActionResult<List<Product>>> SortProducts([FromQuery] string sortBy)
    {
        return await _productService.SortAsync(sortBy);
        //return BadRequest("orderBy parameter is not valid. Should be 'name', 'price' or 'stock'");
    }

    [HttpGet("{id}")]
    // api/Product/1
    // returns a product by id
    public async Task<ActionResult<Product>> GetById(string id)
    {
        var product = await _productService.GetAsync(id);
        return product ?? (ActionResult<Product>)NotFound();
    }

    [HttpGet]
    // api/Product?id=1
    // return a product by id
    public async Task<ActionResult<Product>> GetByIdQuery([FromQuery] string id)
    {
        var product = await _productService.GetAsync(id);
        return product ?? (ActionResult<Product>)NotFound();
    }

    [HttpPost]
    // creates new product 
    public async Task<IActionResult> Post([FromBody] Product product)
    {
        try
        {
            PostProductValidator validator = new PostProductValidator();
            validator.ValidateAndThrow(product);
            await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut("{id}")]
    // updates product
    public async Task<IActionResult> PutProduct([FromRoute] string id, [FromBody] Product product)
    {
        var existingProduct = await _productService.GetAsync(id);
        if (existingProduct is null)
        {
            return NotFound();
        }
        await _productService.UpdateAsync(id, product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingProduct = _productService.GetAsync(id);
        if (existingProduct is null)
        {
            return NotFound();
        }

        await _productService.RemoveAsync(id);
        return NoContent();
    }

    [HttpDelete]
    // deletes a product
    public async Task<IActionResult> DeleteByIdQuery([FromQuery] string id)
    {
        var existingProduct = _productService.GetAsync(id);
        if (existingProduct is null)
        {
            return NotFound();
        }

        await _productService.RemoveAsync(id);
        return NoContent();
    }
}
