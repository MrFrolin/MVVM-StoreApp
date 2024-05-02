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
using DataAccess.Entities;
using DataAccess.Services;
using Labb3Databaser.Enum;
using Labb3Databaser.Manager;
using Labb3Databaser.Models;

namespace Labb3Databaser.Views.CustomerViews
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {
        private readonly UserRepository _userRepo;
        private readonly OrderRepository _orderRepo;

        public Profile()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
            _orderRepo = new OrderRepository();

            DataContext = this;

            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            OrderManager.OrderListChanged += OrderManager_OrderListChanged;
        }

        private void OrderManager_OrderListChanged()
        {
            if (UserManager.LoggedIn)
            {
                SelectedOrderView.Items.Clear();
                OrdersView.Items.Clear();
                foreach (var order in OrderManager.OrderList)
                {
                    OrdersView.Items.Add(order);
                }
            }
        }

        private void UserManager_CurrentUserChanged()
        {
            OrderManager.GetCurrentUserOrderDb(_orderRepo);

            EmailAddressTxt.Text = UserManager.CurrentUser.EmailAddress;
            FirstNameTxt.Text = UserManager.CurrentUser.FirstName;
            LastNameTxt.Text = UserManager.CurrentUser.LastName;

            OrderManager_OrderListChanged();
        }


        private void UpdateCurrentCustomerBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPassword == null)
            {
                MessageBox.Show($"Fill current password to update your profile");
            }
            else if  (EmailAddressTxt.Text != null && FirstNameTxt.Text != null && LastNameTxt.Text != null)
            {
                UserManager.CurrentUser.EmailAddress = EmailAddressTxt.Text;
                UserManager.CurrentUser.FirstName = FirstNameTxt.Text;
                UserManager.CurrentUser.LastName = LastNameTxt.Text;

                if (UserManager.CurrentUser.Authenticate(CurrentPassword.Password) && NewPassword != null && NewPassword.Password == ConfirmPassword.Password)
                {
                    UserManager.CurrentUser.Password = NewPassword.Password;
                }
                else
                {
                    MessageBox.Show($"Incorrect password insert");
                }

                var storeRecord = UserManager.GetUserRecordFromModel(UserManager.CurrentUser);

                UserManager.UpdateUser(UserManager.CurrentUser);
                _userRepo.UpdateUser(storeRecord);
                UserManager.GetAllUsersFromDb(_userRepo);

                MessageBox.Show($"Updated successfully");

            }
            else
            {
                MessageBox.Show($"Profile not updated, missing information");
            }
        }

        private void OrdersView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = OrdersView.SelectedItem as OrderModel;

            if (selectedOrder != null)
            {
                SelectedOrderView.Items.Clear();
                foreach (var product in selectedOrder.OrderedItems)
                {
                    SelectedOrderView.Items.Add(product);
                }
            }
        }

    }
}
