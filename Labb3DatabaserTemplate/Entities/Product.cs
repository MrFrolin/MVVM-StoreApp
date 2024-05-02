using Labb3Databaser.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb3Databaser.DataModels.Products;

public class Product
{

    public ObjectId Id { get; set; }
    public string ProductName { get; set; }
    public double ProductPrice { get; set; }

    [BsonRepresentation(BsonType.String)]
    public ProductType ProductType { get; set; }

    public int ProductCount { get; set; } = 1;
}