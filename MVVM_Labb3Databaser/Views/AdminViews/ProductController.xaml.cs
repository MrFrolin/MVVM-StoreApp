using Labb3Databaser.DataModels.Products;
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
using DataAccess.Services;
using Labb3Databaser.Manager;
using Labb3Databaser.Models;
using Common.DTOs;
using Labb3Databaser.Enum;

namespace Labb3Databaser.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for ProductController.xaml
    /// </summary>
    public partial class ProductController : UserControl
    {
        private readonly ProductRepository _productRepo;
        public ProductModel? SelectedProduct { get; set; }

        public ProductController()
        {
            InitializeComponent();

            DataContext = this;
            _productRepo = new ProductRepository();

            ProductManager.ProductListChanged += ProductManager_ProductListChanged;
            CategoryCoB.ItemsSource = System.Enum.GetValues(typeof(ProductType));
            ProductManager.GetAllProductFromDb(_productRepo);
        }

        private void ProductManager_ProductListChanged()
        {
            ProductView.Items.Clear();
            foreach (var product in ProductManager.ProductList)
            {
                ProductView.Items.Add(product);
            }
        }

        private void ProductView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProduct = ProductView.SelectedItem as ProductModel;

            if (SelectedProduct == null)
            {
                BarcodeTxt.Text = string.Empty;
                ProductNameTxt.Text = string.Empty;
                ProductPriceTxt.Text = string.Empty;
                CategoryCoB.Text = null;
            }
            else
            {
                BarcodeTxt.Text = SelectedProduct.Id;
                ProductNameTxt.Text = SelectedProduct.ProductName;
                ProductPriceTxt.Text = SelectedProduct.ProductPrice.ToString();
                CategoryCoB.SelectedItem = SelectedProduct.ProductType is ProductType ? SelectedProduct.ProductType : ProductType.None;
            }
        }

        private void UpdateAddProductBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct == null)
            {
                if (double.TryParse(ProductPriceTxt.Text, out var parsedPrice) && ProductPriceTxt.Text != string.Empty)
                {
                    var newProdName = ProductNameTxt.Text;
                    var newProdPrice = parsedPrice;
                    var newProdCategory = CategoryCoB.SelectedItem is ProductType ? (ProductType)CategoryCoB.SelectedItem : ProductType.None;
                    var newProdCount = 1;

                    var newProduct = new ProductModel();
                    newProduct.ProductName = newProdName;
                    newProduct.ProductPrice = newProdPrice;
                    newProduct.ProductType = newProdCategory;
                    newProduct.ProductCount = 1;

                    var productRecord = new ProductRecord("", newProdName, newProdPrice, newProdCategory, newProdCount);
                    ProductManager.AddProduct(newProduct);

                    _productRepo.AddProduct(productRecord);

                    ProductNameTxt.Text = string.Empty;
                    ProductPriceTxt.Text = string.Empty;

                    ProductManager.GetAllProductFromDb(_productRepo);
                }
                else
                {
                    MessageBox.Show("Missing information");
                }
            }
            else if (SelectedProduct != null)
            {

                if (double.TryParse(ProductPriceTxt.Text, out var parsedPrice))
                {

                    SelectedProduct.ProductName = ProductNameTxt.Text;
                    SelectedProduct.ProductPrice = parsedPrice;
                    SelectedProduct.ProductType = CategoryCoB.SelectedItem is ProductType ? (ProductType)CategoryCoB.SelectedItem : ProductType.None;

                    var updatedProduct = new ProductRecord(SelectedProduct.Id, SelectedProduct.ProductName, SelectedProduct.ProductPrice, SelectedProduct.ProductType, SelectedProduct.ProductCount);

                    ProductManager.UpdateProduct(SelectedProduct);
                    _productRepo.UpdateProduct(updatedProduct);

                    ProductManager.GetAllProductFromDb(_productRepo);

                }
                else
                {
                    MessageBox.Show("Incorrect price");
                }

            }
        }

        private void DeleteProductBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                _productRepo.DeleteProduct(SelectedProduct.Id);
                ProductManager.RemoveProduct(SelectedProduct.Id);
                
            }
            else
            {
                MessageBox.Show($"Select a product to remove");
            }
        }
    }
}
