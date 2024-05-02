using Labb3Databaser.Enum;

namespace Common.DTOs;

public record ProductRecord(string Id, string ProductName, double ProductPrice, ProductType ProductType, int ProductCount);