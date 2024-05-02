using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Common.DTOs;
using DataAccess.Services;
using Labb3Databaser.Enum;
using Labb3Databaser.Manager;
using Labb3Databaser.Models;
using Labb3Databaser.Views.CustomerViews;

namespace Labb3Databaser.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for StoreController.xaml
    /// </summary>
    public partial class StoreController : UserControl
    {
        private readonly StoreRepository _storeRepo;

        public static StoreModel? SelectedStore { get; set; }

        public StoreController()
        {
            InitializeComponent();

            _storeRepo = new StoreRepository();
            DataContext = this;

            StoreManager.StoreListChanged += StoreManager_StoreListChanged;
            StoreManager.GetAllStoresFromDb(_storeRepo);
        }

        private void StoreManager_StoreListChanged()
        {
            StoreView.Items.Clear();
            foreach (var store in StoreManager.StoreList)
            {
                StoreView.Items.Add(store);
            }
        }

        private void StoreView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStore = StoreView.SelectedItem as StoreModel;

            if (SelectedStore == null)
            {
                StoreIdTxt.Text = string.Empty;
                StoreNameTxt.Text = string.Empty;
                StoreCityTxt.Text = string.Empty;
                StoreAddressTxt.Text = string.Empty;
            }
            else
            {
                StoreIdTxt.Text = SelectedStore.StoreId;
                StoreNameTxt.Text = SelectedStore.StoreName;
                StoreCityTxt.Text = SelectedStore.StoreCity;
                StoreAddressTxt.Text = SelectedStore.StoreAddress;
            }
        }

        private void UpdateAddStoreBtn_OnClick(object sender, RoutedEventArgs e)
        {

            if (SelectedStore == null)
            {
                if (StoreNameTxt.Text == string.Empty || StoreCityTxt.Text == string.Empty || StoreAddressTxt.Text == string.Empty)
                {
                    MessageBox.Show("All fields need to be filled");
                }
                else
                {
                    var newStoreName = StoreNameTxt.Text;
                    var newStoreCity = StoreCityTxt.Text;
                    var newStoreAddress = StoreAddressTxt.Text;

                    var newStore = new StoreModel();
                    newStore.StoreName = newStoreName;
                    newStore.StoreCity = newStoreCity;
                    newStore.StoreAddress = newStoreAddress;
                    newStore.StoreStock = new List<ProductModel>();

                    var storeRecord = new StoreRecord("", newStoreName, newStoreCity, newStoreAddress,
                        new List<ProductRecord>());

                    StoreManager.AddStore(newStore);
                    _storeRepo.AddStore(storeRecord);

                    StoreNameTxt.Text = string.Empty;
                    StoreCityTxt.Text = string.Empty;
                    StoreAddressTxt.Text = string.Empty;

                    StoreManager.GetAllStoresFromDb(_storeRepo);
                }
                
            }
            else if (SelectedStore != null)
            {
                    SelectedStore.StoreName = StoreNameTxt.Text;
                    SelectedStore.StoreCity = StoreCityTxt.Text;
                    SelectedStore.StoreAddress = StoreAddressTxt.Text;

                    var storeRecord = StoreManager.GetStoreRecordFromModel(SelectedStore);

                    StoreManager.UpdateStore(SelectedStore);
                    _storeRepo.UpdateStore(storeRecord);

                    StoreManager.GetAllStoresFromDb(_storeRepo);
                
            }
        }

        private void DeleteStoreBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedStore != null)
            {
                _storeRepo.DeleteStore(SelectedStore.StoreId);
                StoreManager.RemoveStore(SelectedStore.StoreId);

            }
            else
            {
                MessageBox.Show($"Select a Store to remove");
            }
        }
    }
}
