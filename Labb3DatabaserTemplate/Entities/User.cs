using Common.DTOs;
using Labb3Databaser.DataModels.Products;
using Labb3Databaser.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Labb3Databaser.DataModels.Users;

public class User
{
    public ObjectId Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.String)]
    public UserType Type { get; set; }

    public List<ProductRecord> Cart { get; set; } = [];

}