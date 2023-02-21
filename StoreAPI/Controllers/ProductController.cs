using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;
using StoreAPI.Services;

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
        if(product is null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpGet]
    // api/Product?id=1
    // return a product by id
    public async Task<ActionResult<Product>> GetByIdQuery([FromQuery] string id)
    {
        var product = await _productService.GetAsync(id);
        if(product is null)
        {
            return NotFound();
        }
        return product;    }


    [HttpPost]
    // creates new product 
    public async Task<IActionResult> Post([FromBody] Product product)
    {
        await _productService.CreateAsync(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id}, product);
    }

    /*
    [HttpPut]
    // updates product
    public IActionResult PutProduct([FromBody] Product product)
    {
        var existingProduct = Products.SingleOrDefault(p => p.Id == product.Id);
        if (existingProduct is null)
        {
            Products.Add(product);
            lastId++;
            return Created(Request.Path.Value + "/" + product.Id, product);
        }
        var index = Products.IndexOf(existingProduct);
        Products[index] = product;
        return Ok(existingProduct);
    }
    */

    /*
    [HttpPatch]
    // updates changed properties of product
    public IActionResult PatchProduct([FromBody] Product product)
    {
        var existingProduct = Products.SingleOrDefault(p => p.Id == product.Id);
        if (existingProduct is null)
        {
            return NotFound();
        }

        foreach (PropertyInfo prop in product.GetType().GetProperties())
        {
            var propValue = prop.GetValue(product, null);
            if (propValue != null)
            {
                prop.SetValue(existingProduct, propValue);
            }
        }
        return Ok(existingProduct);
    }
    */

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
