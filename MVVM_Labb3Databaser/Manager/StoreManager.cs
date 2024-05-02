using Common.DTOs;
using DataAccess.Services;
using Labb3Databaser.Models;
using System.Collections.ObjectModel;

namespace Labb3Databaser.Manager;

public class StoreManager
{

    public static ObservableCollection<StoreModel> StoreList { get; set; } = new();

    public static event Action StoreListChanged;

    public static void AddStore(StoreModel store)
    {
        StoreList.Add(store);
        StoreListChanged.Invoke();
    }

    public static void RemoveStore(string id)
    {
        var removeStore = StoreList.FirstOrDefault(s => s.StoreId == id);
        StoreList.Remove(removeStore);
        StoreListChanged.Invoke();
    }

    public static void UpdateStore(StoreModel store)
    {
        var existingStore = StoreList.FirstOrDefault(s => s.StoreId == store.StoreId);

        if (existingStore != null)
        {
            existingStore.StoreId = store.StoreId;
            existingStore.StoreName = store.StoreName;
            existingStore.StoreCity = store.StoreCity;
            existingStore.StoreAddress = store.StoreAddress;
            existingStore.StoreStock = store.StoreStock;

            StoreListChanged.Invoke();
        }
    }

    public static void GetAllStoresFromDb(StoreRepository storeRepo)
    {
        var addAllStores = storeRepo.GetAllStores();
        StoreList.Clear();


        foreach (var store in addAllStores)
        {
            var storeStock = new List<ProductModel>();

            foreach (var product in store.Stock)
            {
                storeStock.Add(new ProductModel()
                {
                    ProductName = product.ProductName,
                    Id = product.Id,
                    ProductPrice = product.ProductPrice,
                    ProductType = product.ProductType
                });
            }

            StoreList.Add(new StoreModel() { StoreId = store.Id, StoreName = store.StoreName, StoreCity = store.StoreCity, StoreAddress = store.StoreAddress, StoreStock = storeStock});
            
        }
        StoreListChanged.Invoke();
    }

    public static StoreRecord GetStoreRecordFromModel(StoreModel storeModel)
    {
        var storeStock = new List<ProductRecord>();

        foreach (var product in storeModel.StoreStock)
        {
            storeStock.Add(new ProductRecord(product.Id, product.ProductName, product.ProductPrice,
                product.ProductType, product.ProductCount));
        }

        var storeRecord = new StoreRecord(storeModel.StoreId, storeModel.StoreName, storeModel.StoreCity, storeModel.StoreAddress, storeStock);

        return storeRecord;
    }
}