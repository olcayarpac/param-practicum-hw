using System.ComponentModel.DataAnnotations;

namespace StoreAPI;

public class Product
{
    public int Id { get; set; }

    public double Price { get; set; }

    public int? Stock { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

}
