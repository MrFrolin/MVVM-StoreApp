using Common.DTOs;
using DataAccess.Entities;
using Labb3Databaser.DataModels.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Services;

public class UserRepository
{

    private readonly IMongoCollection<User> _users;

    public UserRepository()
    {
        var hostName = "localhost";
        var port = "27017";
        var databaseName = "PhilipStoreDb";
        var client = new MongoClient($"mongodb://{hostName}:{port}");
        var database = client.GetDatabase(databaseName);
        _users = database.GetCollection<User>("Users", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }

    public void AddUser(UserRecord userRecord)
    {
        var newUser = new User()
        {
            FirstName = userRecord.Firstname,
            LastName = userRecord.Lastname,
            EmailAddress = userRecord.EmailAddress,
            Password = userRecord.Password,
            Type = userRecord.Type,
            Cart = userRecord.Cart
        };

        _users.InsertOne(newUser);
    }

    public List<UserRecord> GetAllUsers()
    {
        var filter = Builders<User>.Filter.Empty;
        var allPeople = _users.Find(filter).ToList()
            .Select(
                u =>
                    new UserRecord(u.Id.ToString(), u.FirstName, u.LastName, u.EmailAddress, u.Password, u.Type, u.Cart)
            );
        return allPeople.ToList();
    }

    public void DeleteUser(string id)
    {
        var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
        _users.DeleteOne(filter);
    }

    public void UpdateUser(UserRecord userRecord)
    {
        var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(userRecord.Id));
        var update = Builders<User>.Update
            .Set(user => user.FirstName, userRecord.Firstname)
            .Set(user => user.LastName, userRecord.Lastname)
            .Set(user => user.Password, userRecord.Password)
            .Set(user => user.EmailAddress, userRecord.EmailAddress)
            .Set(user => user.Type, userRecord.Type)
            .Set(user => user.Cart, userRecord.Cart);

        _users.UpdateOne(filter, update);
    }
}