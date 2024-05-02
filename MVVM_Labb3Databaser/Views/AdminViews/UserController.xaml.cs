using DataAccess.Services;
using Labb3Databaser.Manager;
using Labb3Databaser.Models;
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
using Labb3Databaser.DataModels.Users;
using Labb3Databaser.Enum;

namespace Labb3Databaser.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for UserController.xaml
    /// </summary>
    public partial class UserController : UserControl
    {

        private readonly UserRepository _userRepo;

        public UserModel? SelectedUser { get; set; }

        public UserController()
        {
            InitializeComponent();

            _userRepo = new UserRepository();
            DataContext = this;

            UserManager.UserListChanged += UserManager_UserListChanged;
            UserManager.GetAllUsersFromDb(_userRepo);

        }

        private void UserManager_UserListChanged()
        {
            UsersView.Items.Clear();
            foreach (var user in UserManager.UserList)
            {
                UsersView.Items.Add(user);
            }
        }

        private void UsersView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedUser = UsersView.SelectedItem as UserModel;

            if (SelectedUser != null)
            {
                UserIDTxt.Text = SelectedUser.Id;
                EmailAddressTxt.Text = SelectedUser.EmailAddress;
                FirstNameTxt.Text = SelectedUser.FirstName;
                LastNameTxt.Text = SelectedUser.LastName;
                PasswordTxt.Password = SelectedUser.Password;

                if (SelectedUser.Type is UserType.Admin)
                {
                    IsAdmin.IsChecked = true;
                }
                else
                {
                    IsAdmin.IsChecked = false;
                }
            }
            else
            {
                UserIDTxt.Text = string.Empty;
                EmailAddressTxt.Text = string.Empty;
                FirstNameTxt.Text = string.Empty;
                LastNameTxt.Text = string.Empty;
                PasswordTxt.Password = string.Empty;
                IsAdmin.IsChecked = false;
            }
        }

        private void UpdateAddUserBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null)
            {
                if (EmailAddressTxt.Text != string.Empty && FirstNameTxt.Text != string.Empty && LastNameTxt.Text != string.Empty && PasswordTxt.Password != string.Empty)
                {

                    var regEmail = EmailAddressTxt.Text;
                    var regFirstName = FirstNameTxt.Text;
                    var regLastName = LastNameTxt.Text;
                    var regPassword = PasswordTxt.Password;
                    var regType = UserType.Customer;

                    if (IsAdmin.IsChecked == true)
                    {
                        regType = UserType.Admin;
                    }

                    var existingUser = UserManager.UserList.Any(customer => customer.EmailAddress == regEmail);
                    if (!existingUser)
                    {
                        var newUser = new UserModel();
                        newUser.FirstName = regFirstName;
                        newUser.LastName = regLastName;
                        newUser.EmailAddress = regEmail;
                        newUser.Password = regPassword;

                        var userRecord = new UserRecord("", regFirstName, regLastName, regEmail, regPassword, regType, new List<ProductRecord>());

                        UserManager.AddUser(newUser);
                        _userRepo.AddUser(userRecord);

                        EmailAddressTxt.Text = string.Empty;
                        FirstNameTxt.Text = string.Empty;
                        LastNameTxt.Text = string.Empty;
                        PasswordTxt.Password = string.Empty;
                        IsAdmin.IsChecked = false;
                        UserManager.GetAllUsersFromDb(_userRepo);
                    }
                    else
                    {
                        MessageBox.Show("Email already in use", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"Customer information needs to be filled");
                }
            }
            else
            {
                SelectedUser.EmailAddress = EmailAddressTxt.Text;
                SelectedUser.FirstName = FirstNameTxt.Text;
                SelectedUser.LastName = LastNameTxt.Text;
                SelectedUser.Password = PasswordTxt.Password;
                SelectedUser.Type = UserType.Customer;

                if (IsAdmin.IsChecked == true)
                {
                    SelectedUser.Type = UserType.Admin;
                }

                var storeRecord = UserManager.GetUserRecordFromModel(SelectedUser);

                UserManager.UpdateUser(SelectedUser);
                _userRepo.UpdateUser(storeRecord);

                UserManager.GetAllUsersFromDb(_userRepo);
            }
        }

        private void DeleteUserBtn_OnClick(object sender, RoutedEventArgs e)
        {

            if (SelectedUser != null)
            {
                var numberOfAdmins = UserManager.UserList.Count(u => u.Type == UserType.Admin);

                if (numberOfAdmins == 1 && SelectedUser.Type == UserType.Admin)
                {
                    MessageBox.Show($"The application needs at least one Admin");
                }
                else
                {
                    _userRepo.DeleteUser(SelectedUser.Id);
                    UserManager.RemoveUser(SelectedUser);
                }
            }
            else
            {
                MessageBox.Show($"Select a user to remove");
            }
        }
    }
}
