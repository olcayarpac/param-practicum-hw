using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StoreAPI.Models;

namespace StoreAPI.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductService(IOptions<StoreDatabaseSettings> storeDatabaseSettings)
    {
        var mongoClient = new MongoClient(storeDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(storeDatabaseSettings.Value.DatabaseName);
        _productsCollection = mongoDatabase.GetCollection<Product>(storeDatabaseSettings.Value.ProductsCollectionName);
    }

    public async Task<List<Product>> GetAsync() =>
        await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetAsync(string id) =>
        await _productsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedBook) =>
        await _productsCollection.ReplaceOneAsync(p => p.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _productsCollection.DeleteOneAsync(p => p.Id == id);

    public async Task<List<Product>> SortAsync(string sortBy)
    {
        var filter = Builders<Product>.Filter.Empty;
        var sortQuery = Builders<Product>.Sort.Ascending(sortBy);
        return (await _productsCollection.FindAsync(filter, new FindOptions<Product, Product>()
        {
            Sort = sortQuery
        })).ToList();
    }

}