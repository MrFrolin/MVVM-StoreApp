using Common.DTOs;
using Labb3Databaser.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DataAccess.Entities;

public class Order
{
    public ObjectId Id { get; set; }
    public string UserId { get; set; }
    public string OrderDate { get; set; }
    public string DeliveryAddress { get; set; }
    public string ZipCode { get; set; }
    public List<ProductRecord> OrderedItems { get; set; } = [];
    public double OrderValue { get; set; }

}