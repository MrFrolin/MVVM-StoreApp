using System.Text.Json.Serialization;

namespace Labb3Databaser.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    Customer,
    Admin
    
}