using System.Collections.ObjectModel;
using DataAccess.Services;
using Labb3Databaser.DataModels.Products;
using Labb3Databaser.Models;
using Labb3Databaser.Views.AdminViews;

namespace Labb3Databaser.Manager;

public class ProductManager
{
    public static ObservableCollection<ProductModel> ProductList { get; set; } = new();

    public static event Action ProductListChanged;

    public static void AddProduct(ProductModel product)
    {
        ProductList.Add(product);
        ProductListChanged.Invoke();
    }

    public static void RemoveProduct(string id)
    {
        var removeProduct = ProductList.FirstOrDefault(p => p.Id == id);
        ProductList.Remove(removeProduct);
        ProductListChanged.Invoke();
    }

    public static void UpdateProduct(ProductModel product)
    {
        var existingProduct = ProductList.FirstOrDefault(p => p.Id == product.Id);

        if (existingProduct != null)
        {
            existingProduct.Id = product.Id;
            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.ProductType = product.ProductType;
            ProductListChanged.Invoke();
        }
    }

    public static void GetAllProductFromDb(ProductRepository prodRepo)
    {
        var addAllProducts = prodRepo.GetAllProducts();
        ProductList.Clear();
        foreach (var product in addAllProducts)
        {
            ProductList.Add(new ProductModel() { Id = product.Id, ProductName = product.ProductName, ProductPrice = product.ProductPrice, ProductType = product.ProductType });
            ProductListChanged.Invoke();
        }
    }
}