using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace StoreAPI.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class ProductController : ControllerBase
{
    private static List<Product> Products = new List<Product>
    {
        new Product{Id = 1, Price = 25699, Name = "IPhone 12", Stock = 5 },
        new Product{Id = 2, Price = 36999, Name = "Macbook Air", Stock = 5 },
        new Product{Id = 3, Price = 42699, Name = "Macbook Pro", Stock = 5 },
        new Product{Id = 4, Price = 5999, Name = "Airpods", Stock = 5 },
    };
    // id counter
    private int lastId = 5;

    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    [HttpGet("products")]
    public IEnumerable<Product> Get()
    {
        return Products;
    }

    [HttpGet("list")]
    // api/Product/list?orderBy=name
    // returns products as ordered by desired property
    public IActionResult ListProducts([FromQuery] string orderBy)
    {
        if(orderBy == "name"){
            return Ok(Products.OrderBy(p => p.Name));
        }
        else if(orderBy == "price"){
            return Ok(Products.OrderBy(p => p.Price));
        }
        else if(orderBy == "stock"){
            return Ok(Products.OrderBy(p => p.Stock));
        }        return BadRequest("orderBy parameter is not valid. Should be 'name', 'price' or 'stock'");
    }

    [HttpGet("{id}")]
    // api/Product/1
    // returns a product by id
    public IEnumerable<Product> GetById(int id)
    {
        return Products.Where(p => p.Id == id).ToArray();
    }

    [HttpGet]
    // api/Product?id=1
    // return a product by id
    public IEnumerable<Product> GetByIdQuery([FromQuery] int id)
    {
        return Products.Where(p => p.Id == id).ToArray();
    }

    [HttpPost]
    // creates new product 
    public IActionResult PostProduct([FromBody] Product product)
    {
        var existingProduct = Products.SingleOrDefault(p => p.Name == product.Name);
        if (existingProduct is not null)
        {
            return Conflict("Product already exists");
        }
        product.Id = lastId;
        this.lastId++;
        Products.Add(product);
        return Created(Request.Path.Value + "/" + product.Id, product);
    }

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
            if(propValue != null){
                prop.SetValue(existingProduct, propValue);
            }
        }
        return Ok(existingProduct);
    }

    [HttpDelete]
    // deletes a product
    public IActionResult DeleteProduct([FromBody] Product product)
    {
        var existingProduct = Products.SingleOrDefault(p => p.Id == product.Id);
        if (existingProduct is null)
        {
            return NotFound();
        }

        Products.Remove(existingProduct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProductById(int id)
    {
        var existingProduct = Products.SingleOrDefault(p => p.Id == id);
        if (existingProduct is null)
        {
            return NotFound();
        }

        Products.Remove(existingProduct);
        return NoContent();
    }
}
