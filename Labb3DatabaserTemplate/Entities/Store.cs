using Common.DTOs;
using Labb3Databaser.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Labb3Databaser.DataModels.Products;

namespace DataAccess.Entities;

public class Store
{
    public ObjectId Id { get; set; }
    public string StoreName { get; set; }
    public string StoreCity { get; set; }
    public string StoreAddress { get; set; }
    public List<ProductRecord> Stock { get; set; } = [];
}