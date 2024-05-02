using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using Common.DTOs;
using DataAccess.Entities;
using DataAccess.Services;
using Labb3Databaser.DataModels.Products;
using Labb3Databaser.Enum;
using Labb3Databaser.Manager;
using Labb3Databaser.Models;
using Labb3Databaser.Views.CustomerViews;
using static System.Net.Mime.MediaTypeNames;

namespace Labb3Databaser.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for StockController.xaml
    /// </summary>
    public partial class StockController : UserControl
    {

        private readonly StoreRepository _storeRepo;

        public StoreModel? SelectedStore { get; set; }
        public ProductModel? SelectedProductCL { get; set; }
        public ProductModel? SelectedProductStoreStock { get; set; }

        public StockController()
        {
            InitializeComponent();
            DataContext = this;
            _storeRepo = new StoreRepository();

            ProductManager.ProductListChanged += ProductManager_ProductListChanged;
            StoreManager.StoreListChanged += StoreManager_StoreListChanged;

        }

        private void ProductManager_ProductListChanged()
        {
            ProductsView.Items.Clear();
            foreach (var product in ProductManager.ProductList)
            {
                ProductsView.Items.Add(product);
            }
        }

        private void StoreManager_StoreListChanged()
        {

            var comboBoxSelectedItem = StoresComboBox.SelectedItem;

            if (StoresComboBox.Items.Count == 0 || StoresComboBox.Items.Count > StoreManager.StoreList.Count)
            {
                StoresComboBox.Items.Clear();
                foreach (var store in StoreManager.StoreList)
                {
                    StoresComboBox.Items.Add(store);
                }
            }
            else if (StoresComboBox.Items.Count < StoreManager.StoreList.Count)
            {
                var newStore = StoreManager.StoreList.Last();
                StoresComboBox.Items.Add(newStore);
            }

            StoresComboBox.SelectedIndex = -1;
            StoresComboBox.SelectedItem = comboBoxSelectedItem;
        }

        private void StoresComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStore = StoresComboBox.SelectedItem as StoreModel;

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
                SelectedStore = null;
                StoreStock.Items.Clear();
            }

        }

        private void ProductsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProductCL = ProductsView.SelectedItem as ProductModel;
        }

        private void AddToStoreBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QtyBox.Text, out var qtyBoxIsInt) && SelectedStore != null && SelectedProductCL != null)
            {
                var selectedStoreComboBox = SelectedStore;

                var productExistInStock = SelectedStore.StoreStock.FirstOrDefault(p => p.Id == SelectedProductCL.Id);

                if (productExistInStock != null)
                {
                    productExistInStock.ProductCount += qtyBoxIsInt;
                }
                else
                {
                    SelectedProductCL.ProductCount = qtyBoxIsInt;
                    SelectedStore.StoreStock.Add(SelectedProductCL);
                }

                var storeRecord = StoreManager.GetStoreRecordFromModel(SelectedStore);

                _storeRepo.UpdateStore(storeRecord);
                StoreManager.UpdateStore(SelectedStore);

                StoresComboBox.SelectedItem = selectedStoreComboBox;
            }
            else
            {
                MessageBox.Show("Select a product, qty value and store");
            }
        }

        private void StoreStock_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProductStoreStock = StoreStock.SelectedItem as ProductModel;
        }

        private void RemoveFromSelectedStore_OnClick(object sender, RoutedEventArgs e)
        {

            if (SelectedStore != null && SelectedProductStoreStock != null)
            {
                var selectedStoreComboBox = SelectedStore;
                var productExistInStock = SelectedStore.StoreStock.FirstOrDefault(p => p.Id == SelectedProductStoreStock.Id);

                SelectedStore.StoreStock.Remove(productExistInStock);

                var storeRecord = StoreManager.GetStoreRecordFromModel(SelectedStore);

                _storeRepo.UpdateStore(storeRecord);
                StoreManager.UpdateStore(SelectedStore);

                StoresComboBox.SelectedItem = selectedStoreComboBox;
            }
            else
            {
                MessageBox.Show($"Select a store and a product");
            }
        }

        private void StockUpdBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedStore != null && SelectedProductStoreStock != null)
            {
                var productExistInStock = SelectedStore.StoreStock.FirstOrDefault(p => p.Id == SelectedProductStoreStock.Id);

                if (int.TryParse(StockBox.Text, out var stockBoxValue) && productExistInStock != null)
                {
                    var selectedStoreComboBox = SelectedStore;
                    productExistInStock.ProductCount = stockBoxValue;

                    var storeRecord = StoreManager.GetStoreRecordFromModel(SelectedStore);

                    _storeRepo.UpdateStore(storeRecord);
                    StoreManager.UpdateStore(SelectedStore);

                    StoresComboBox.SelectedItem = selectedStoreComboBox;
                }
                else
                {
                    MessageBox.Show("Add stock value");
                }
            }
            else
            {
                MessageBox.Show($"Select a store and product");
            }

        }
    }
}
