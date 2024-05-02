using Common.DTOs;
using Labb3Databaser.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labb3Databaser.Models;

public class OrderModel
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string OrderDate { get; set; }
    public string DeliveryAddress { get; set; }
    public string ZipCode { get; set; }
    public List<ProductModel> OrderedItems { get; set; } = [];
    public double OrderValue { get; set; }
}