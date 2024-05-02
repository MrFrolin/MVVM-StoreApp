using System.Windows;
using System.Windows.Controls;
using Common.DTOs;
using DataAccess.Services;
using Labb3Databaser.Enum;
using Labb3Databaser.Manager;
using Labb3Databaser.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Labb3Databaser.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private readonly UserRepository _userRepo;

        public LoginView()
        {
            InitializeComponent();
            _userRepo = new UserRepository();

            UserManager.GetAllUsersFromDb(_userRepo);
        }

        private void RegisterCustomerBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var regEmail = RegisterEmail.Text;
            var regPassword = RegisterPwd.Password;
            var regFirstName = RegisterFirstname.Text;
            var regLastName = RegisterLastname.Text;

            if (regEmail == string.Empty || regFirstName == string.Empty || regLastName == string.Empty)
            {
                MessageBox.Show("Fields needs to be filed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (regPassword.Length < 3)
            {
                MessageBox.Show("Password must be at least 3 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var existingUser = UserManager.UserList.Any(customer => customer.EmailAddress == regEmail);
                if (!existingUser)
                {
                    var newUser = new UserModel();
                    newUser.FirstName = regFirstName;
                    newUser.LastName = regLastName;
                    newUser.EmailAddress = regEmail;
                    newUser.Password = regPassword;
                    newUser.Type = UserType.Admin;

                    var userRecord = new UserRecord("", regFirstName, regLastName, regEmail, regPassword, UserType.Admin, new List<ProductRecord>());
                    _userRepo.AddUser(userRecord);
                    UserManager.AddUser(newUser);

                    UserManager.GetAllUsersFromDb(_userRepo);

                    RegisterEmail.Text = string.Empty;
                    RegisterPwd.Password = string.Empty;
                    RegisterFirstname.Text = string.Empty;
                    RegisterLastname.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Email already in use", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var emailAddress = EmailAddress.Text;
            var loginPassword = LoginPwd.Password;

            var user = UserManager.UserList?.FirstOrDefault(loginUser => loginUser.EmailAddress == emailAddress);
            if (user != null)
            {
                if (user.Authenticate(loginPassword))
                {
                    UserManager.LoggedIn = true;
                    UserManager.ChangeCurrentUser(user.EmailAddress);
                    EmailAddress.Text = string.Empty;
                    LoginPwd.Password = string.Empty;

                }
                else
                {
                    MessageBox.Show("Wrong password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("User does not exist, register", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
