using Labb3Databaser.Models;
using System.Collections.ObjectModel;
using Common.DTOs;
using DataAccess.Services;
using Labb3Databaser.DataModels.Products;

namespace Labb3Databaser.Manager;

public class OrderManager
{
    public static ObservableCollection<OrderModel> OrderList { get; set; } = new();

    public static event Action OrderListChanged;

    public static void AddOrder(OrderModel order)
    {
        OrderList.Add(order);
        OrderListChanged.Invoke();
    }

    public static void RemoveOrder(string id)
    {
        var removeOrder = OrderList.FirstOrDefault(o => o.Id == id);
        OrderList.Remove(removeOrder);
        OrderListChanged.Invoke();
    }

    public static OrderRecord GetOrderRecordFromModel(OrderModel order)
    {
        var orderedItems = new List<ProductRecord>();

        foreach (var product in order.OrderedItems)
        {
            orderedItems.Add(new ProductRecord(product.Id, product.ProductName, product.ProductPrice,
                product.ProductType, product.ProductCount));
        }

        var newOrderRecord = new OrderRecord(order.Id, order.UserId, order.OrderDate, order.DeliveryAddress, order.ZipCode, orderedItems, order.OrderValue);

        return newOrderRecord;
    }

    public static void GetCurrentUserOrderDb(OrderRepository orderRepo)
    {
        var addAllOrders = orderRepo.GetCurrentUserOrders(UserManager.CurrentUser.Id);
        OrderList.Clear();
        foreach (var order in addAllOrders)
        {

            var orderItems = new List<ProductModel>();

            foreach (var product in order.OrderedItems)
            {
                orderItems.Add(new ProductModel()
                {
                    ProductName = product.ProductName,
                    Id = product.Id,
                    ProductPrice = product.ProductPrice,
                    ProductType = product.ProductType
                });
            }

            OrderList.Add(new OrderModel()
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                DeliveryAddress = order.DeliveryAddress,
                ZipCode = order.ZipCode,
                OrderedItems = orderItems,
                OrderValue = order.OrderValue
            });
            OrderListChanged.Invoke();
        }
    }
}


