namespace Common.DTOs;

public record StoreRecord(string Id, string StoreName, string StoreCity, string StoreAddress, List<ProductRecord> Stock);