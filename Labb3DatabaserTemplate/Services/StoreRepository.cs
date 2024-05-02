using Common.DTOs;
using DataAccess.Entities;
using Labb3Databaser.DataModels.Products;
using Labb3Databaser.DataModels.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Services;

public class StoreRepository
{
    private readonly IMongoCollection<Store> _store;

    public StoreRepository()
    {
        var hostName = "localhost";
        var port = "27017";
        var databaseName = "PhilipStoreDb";
        var client = new MongoClient($"mongodb://{hostName}:{port}");
        var database = client.GetDatabase(databaseName);
        _store = database.GetCollection<Store>("Stores", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }

    public void AddStore(StoreRecord storeRecord)
    {
        var newStore = new Store()
        {
            StoreName = storeRecord.StoreName,
            StoreCity = storeRecord.StoreCity,
            StoreAddress = storeRecord.StoreAddress,
            Stock = storeRecord.Stock
        };

        _store.InsertOne(newStore);
    }

    public List<StoreRecord> GetAllStores()
    {
        var filter = Builders<Store>.Filter.Empty;
        var allStores = _store.Find(filter).ToList()
            .Select(
                p =>
                    new StoreRecord(p.Id.ToString(), p.StoreName, p.StoreCity, p.StoreAddress, p.Stock)
            );
        return allStores.ToList();
    }

    public void DeleteStore(string id)
    {
        var filter = Builders<Store>.Filter.Eq("_id", ObjectId.Parse(id));
        _store.DeleteOne(filter);
    }

    public void UpdateStore(StoreRecord storeRecord)
    {
        var filter = Builders<Store>.Filter.Eq("_id", ObjectId.Parse(storeRecord.Id));
        var update = Builders<Store>.Update
            .Set(store => store.StoreName, storeRecord.StoreName)
            .Set(store => store.StoreCity, storeRecord.StoreCity)
            .Set(store => store.StoreAddress, storeRecord.StoreAddress)
            .Set(store => store.Stock, storeRecord.Stock);

        _store.UpdateOne(filter, update);
    }
}