using Labb3Databaser.Enum;

namespace Common.DTOs;

public record OrderRecord(string Id, string UserId, string OrderDate, string DeliveryAddress, string ZipCode, List<ProductRecord> OrderedItems, double OrderValue);
