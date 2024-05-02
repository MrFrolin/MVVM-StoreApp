using Common.DTOs;
using MongoDB.Driver;
using Labb3Databaser.DataModels.Products;
using Labb3Databaser.DataModels.Users;
using MongoDB.Bson;

namespace DataAccess.Services;

public class ProductRepository
{
    private readonly IMongoCollection<Product> _product;

    public ProductRepository()
    {
        var hostName = "localhost";
        var port = "27017";
        var databaseName = "PhilipStoreDb";
        var client = new MongoClient($"mongodb://{hostName}:{port}");
        var database = client.GetDatabase(databaseName);
        _product = database.GetCollection<Product>("Products", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }

    public void AddProduct(ProductRecord productRecord)
    {
        var newProduct = new Product()
        {
            ProductName = productRecord.ProductName,
            ProductPrice = productRecord.ProductPrice,
            ProductType = productRecord.ProductType
        };

        _product.InsertOne(newProduct);
    }

    public List<ProductRecord> GetAllProducts()
    {
        var filter = Builders<Product>.Filter.Empty;
        var allProducts = _product.Find(filter).ToList()
            .Select(
                p =>
                    new ProductRecord(p.Id.ToString(), p.ProductName, p.ProductPrice, p.ProductType, p.ProductCount)
            );
        return allProducts.ToList();
    }

    public void DeleteProduct(string id)
    {
            var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(id));
            _product.DeleteOne(filter);
    }

    public void UpdateProduct(ProductRecord productRecord)
    {
        var filter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(productRecord.Id));
        var update = Builders<Product>.Update
            .Set(product => product.ProductName, productRecord.ProductName)
            .Set(product => product.ProductPrice, productRecord.ProductPrice)
            .Set(product => product.ProductType, productRecord.ProductType);

        _product.UpdateOne(filter, update);
    }
}