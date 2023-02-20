using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreAPI.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string? ProductName { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int StockAmount {get; set;} 
}