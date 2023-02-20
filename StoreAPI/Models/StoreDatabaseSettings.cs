namespace StoreAPI.Models;

public class StoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string ProductsCollectionName { get; set; } = null!;
    public string OrdersCollectionName { get; set; } = null!;
    public string SellersCollectionName { get; set; } = null!;

}