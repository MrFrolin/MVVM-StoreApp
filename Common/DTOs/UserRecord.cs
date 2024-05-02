using Labb3Databaser.Enum;

namespace Common.DTOs;

public record UserRecord(string Id, string Firstname, string Lastname, string EmailAddress, string Password, UserType Type, List<ProductRecord> Cart);