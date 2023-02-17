using Microsoft.AspNetCore.Mvc;

namespace StoreAPI.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class ProductController : ControllerBase
{
    private static readonly Product[] Products = new[]
    {
        new Product{Id = 1, Price = 25699, Name = "IPhone 12", Stock = 5 },
        new Product{Id = 2, Price = 36999, Name = "Macbook Air", Stock = 5 },
        new Product{Id = 3, Price = 42699, Name = "Macbook Pro", Stock = 5 },
        new Product{Id = 4, Price = 5999, Name = "Airpods", Stock = 5 },
    };

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

    [HttpGet("{id}")]
    public IEnumerable<Product> GetById(int id)
    {
        return Products.Where(p => p.Id == id).ToArray();
    }

    [HttpGet]
    public IEnumerable<Product> GetByIdQuery([FromQuery] int id)
    {
        return Products.Where(p => p.Id == id).ToArray();
    }

}
