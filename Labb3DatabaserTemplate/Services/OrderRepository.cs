using Common.DTOs;
using DataAccess.Entities;
using Labb3Databaser.DataModels.Products;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace DataAccess.Services;

public class OrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public OrderRepository()
    {
        var hostName = "localhost";
        var port = "27017";
        var databaseName = "PhilipStoreDb";
        var client = new MongoClient($"mongodb://{hostName}:{port}");
        var database = client.GetDatabase(databaseName);
        _orders = database.GetCollection<Order>("Orders", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }

    public void AddOrder(OrderRecord orderRecord)
    {
        var newOrder = new Order()
        {
            UserId = orderRecord.UserId,
            OrderDate = orderRecord.OrderDate,
            DeliveryAddress = orderRecord.DeliveryAddress,
            ZipCode = orderRecord.ZipCode,
            OrderedItems = orderRecord.OrderedItems,
            OrderValue = orderRecord.OrderValue,
        };

        _orders.InsertOne(newOrder);
    }

    public void DeleteOrder(string id)
    {
        var filter = Builders<Order>.Filter.Eq("_id", ObjectId.Parse(id));
        _orders.DeleteOne(filter);
    }

    public List<OrderRecord> GetCurrentUserOrders(string userId)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.UserId, userId);
        var allOrders = _orders.Find(filter).ToList()
            .Select(
                o =>
                    new OrderRecord(o.Id.ToString(), o.UserId,o.OrderDate, o.DeliveryAddress, o.ZipCode, o.OrderedItems, o.OrderValue)
            );
        return allOrders.ToList();
    }
}