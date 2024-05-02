using DataAccess.Services;
using Labb3Databaser.DataModels.Products;
using Labb3Databaser.Manager;
using Labb3Databaser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Common.DTOs;
using Labb3Databaser.Enum;

namespace Labb3Databaser.Views.CustomerViews
{
    /// <summary>
    /// Interaction logic for CheckoutView.xaml
    /// </summary>
    public partial class CheckoutView : UserControl
    {

        private readonly UserRepository _userRepo;
        private readonly OrderRepository _orderRepo;

        public ProductModel? SelectedProductCart { get; set; }



        public CheckoutView()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
            _orderRepo = new OrderRepository();

            DataContext = this;

            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;


        }

        private void UserManager_CurrentUserChanged()
        {
            if (!UserManager.LoggedIn)
            {
                CartView.Items.Clear();
            }
            else
            {
                double cartValue = 0;
                CartView.Items.Clear();
                if (UserManager.CurrentUser.Cart != null)
                {
                    foreach (var product in UserManager.CurrentUser.Cart)
                    {
                        CartView.Items.Add(product);

                        cartValue += (product.ProductCount * product.ProductPrice);
                    }
                    CartValue.Content = cartValue;
                    TotalAmount.Content = cartValue + 49;
                }
            }
        }

        private void RemoveFromCart_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedProductCart != null)
            {
                var selectedProductToRemove = UserManager.CurrentUser.Cart.FirstOrDefault(p => p.Id == SelectedProductCart.Id);

                if (selectedProductToRemove != null)
                {
                    UserManager.CurrentUser.Cart.Remove(selectedProductToRemove);
                    UserManager.UpdateUser(UserManager.CurrentUser);

                    var userRecord = UserManager.GetUserRecordFromModel(UserManager.CurrentUser);

                    _userRepo.UpdateUser(userRecord);
                }
            }
            else
            {
                MessageBox.Show("Select a product");
            }
        }

        private void QtyUpdBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var productExistInCart = UserManager.CurrentUser.Cart.FirstOrDefault(p => p.Id == SelectedProductCart.Id);

            if (int.TryParse(QtyBox.Text, out var qtyBoxValue) && productExistInCart != null)
            {

                productExistInCart.ProductCount = qtyBoxValue;
                var userRecord = UserManager.GetUserRecordFromModel(UserManager.CurrentUser);

                _userRepo.UpdateUser(userRecord);
                UserManager.UpdateUser(UserManager.CurrentUser);
            }
            else
            {
                MessageBox.Show("Add qty value");
            }
        }

        private void CartView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProductCart = CartView.SelectedItem as ProductModel;
        }

        private void CheckoutBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (UserManager.CurrentUser.Cart.Count != 0)
            {
                if (DeliveryTxt.Text != string.Empty && ZipTxt.Text != string.Empty)
                {
                    var userId = UserManager.CurrentUser.Id;
                    var deliveryAddress = DeliveryTxt.Text;
                    var zipCode = ZipTxt.Text;
                    var orderedItems = UserManager.CurrentUser.Cart;
                    orderedItems.Add(new ProductModel() { Id = "Shipping", ProductCount = 1, ProductName = "Shipping", ProductPrice = 49, ProductType = ProductType.None });
                    double orderValue = 0;
                    foreach (var product in orderedItems)
                    {
                        orderValue += (product.ProductPrice * product.ProductCount);
                    }

                    var orderModel = new OrderModel();
                    orderModel.UserId = userId;
                    orderModel.DeliveryAddress = deliveryAddress;
                    orderModel.ZipCode = zipCode;
                    orderModel.OrderValue = orderValue;
                    orderModel.OrderedItems = orderedItems;

                    var userRecord = UserManager.GetUserRecordFromModel(UserManager.CurrentUser);

                    var orderRecord = new OrderRecord("", userId, DateOnly.FromDateTime(DateTime.Today).ToString(), deliveryAddress,
                        zipCode, userRecord.Cart, orderValue);

                    OrderManager.AddOrder(orderModel);
                    _orderRepo.AddOrder(orderRecord);

                    DeliveryTxt.Text = string.Empty;
                    ZipTxt.Text = string.Empty;

                    UserManager.CurrentUser.Cart.Clear();
                    var storeRecord = UserManager.GetUserRecordFromModel(UserManager.CurrentUser);
                    UserManager.UpdateUser(UserManager.CurrentUser);
                    _userRepo.UpdateUser(storeRecord);
                    UserManager.GetAllUsersFromDb(_userRepo);
                }
                else
                {
                    MessageBox.Show($"Missing delivery information");
                }
            }
            else
            {
                MessageBox.Show($"Cart is empty");
            }

            

        }
    }
}
