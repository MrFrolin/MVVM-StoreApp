using Common.DTOs;
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
using DataAccess.Services;

namespace Labb3Databaser.Views.CustomerViews
{
    /// <summary>
    /// Interaction logic for ShopView.xaml
    /// </summary>
    public partial class ShopView : UserControl
    {

        private readonly UserRepository _userRepo;

        public ProductModel? SelectedProductStore { get; set; }

        public ProductModel? SelectedProductCart { get; set; }

        public static StoreModel SelectedStore { get; set; }

        public ShopView()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
            StoreManager_StoreListChanged();

            DataContext = this;

            StoreManager.StoreListChanged += StoreManager_StoreListChanged;
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;

        }

        private void UserManager_CurrentUserChanged()
        {
            if (!UserManager.LoggedIn)
            {
                CartView.Items.Clear();
            }
            else if (UserManager.CurrentUser != null)
            {
                CartView.Items.Clear();
                if (UserManager.CurrentUser.Cart != null)
                {
                    foreach (var product in UserManager.CurrentUser.Cart)
                    {
                        CartView.Items.Add(product);
                    }
                }
            }
        }

        private void StoreManager_StoreListChanged()
        {
            
            if (StoresComboBoxCustomerView.Items.Count == 0 || StoresComboBoxCustomerView.Items.Count > StoreManager.StoreList.Count)
            {
                StoresComboBoxCustomerView.Items.Clear();
                foreach (var store in StoreManager.StoreList)
                {
                    StoresComboBoxCustomerView.Items.Add(store);
                }
            }
            else if (StoresComboBoxCustomerView.Items.Count < StoreManager.StoreList.Count)
            {
                var newStore = StoreManager.StoreList.Last();
                StoresComboBoxCustomerView.Items.Add(newStore);
            }
        }

        private void CartViewView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProductCart = CartView.SelectedItem as ProductModel;
        }

        private void StoresComboBoxCustomerView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStore = StoresComboBoxCustomerView.SelectedItem as StoreModel;

            if (SelectedStore != null)
            {
                StoreStock.Items.Clear();
                foreach (var product in SelectedStore.StoreStock)
                {
                    StoreStock.Items.Add(product);
                }
            }
            else
            {
                StoreStock.Items.Clear();
            }


        }

        private void StoreStock_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProductStore = StoreStock.SelectedItem as ProductModel;
        }

        private void AddToCartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(StockBox.Text, out var qtyBoxIsInt) && SelectedStore != null && SelectedProductStore != null)
            {
                var selectedStoreComboBox = SelectedStore;

                var productExistInCart = UserManager.CurrentUser.Cart.FirstOrDefault(p => p.Id == SelectedProductStore.Id);

                if (productExistInCart != null)
                {
                    productExistInCart.ProductCount += qtyBoxIsInt;
                }
                else
                {
                    SelectedProductStore.ProductCount = qtyBoxIsInt;
                    UserManager.CurrentUser.Cart.Add(SelectedProductStore);
                }

                var userRecord = UserManager.GetUserRecordFromModel(UserManager.CurrentUser);

                _userRepo.UpdateUser(userRecord);
                UserManager.UpdateUser(UserManager.CurrentUser);

                StoresComboBoxCustomerView.SelectedItem = selectedStoreComboBox;
            }
            else
            {
                MessageBox.Show("Select a store and product to add");
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

            if (SelectedProductCart != null)
            {
                var productExistInCart = UserManager.CurrentUser.Cart.FirstOrDefault(p => p.Id == SelectedProductCart.Id);

                if (int.TryParse(QtyBox.Text, out var qtyBoxValue) && productExistInCart != null)
                {
                    var selectedStoreComboBox = SelectedStore;

                    productExistInCart.ProductCount = qtyBoxValue;
                    var userRecord = UserManager.GetUserRecordFromModel(UserManager.CurrentUser);

                    _userRepo.UpdateUser(userRecord);
                    UserManager.UpdateUser(UserManager.CurrentUser);

                    StoresComboBoxCustomerView.SelectedItem = selectedStoreComboBox;
                }
                else
                {
                    MessageBox.Show("Add qty value");
                }
            }
            else
            {
                MessageBox.Show("Select a product");
            }
        }
    }
}
