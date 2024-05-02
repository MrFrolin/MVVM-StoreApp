using Labb3Databaser.Manager;
using Labb3Databaser.Views.AdminViews;
using System.Windows;
using System.Windows.Controls;

namespace Labb3Databaser.Views
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
        }

        private void UserManager_CurrentUserChanged()
        {
            if (UserManager.LoggedIn)
            {
                UserName.Content = $"{UserManager.CurrentUser.FirstName}";
            }
            else
            {
                UserName.Content = string.Empty;
            }
        }

        private void LogOutBtn_OnClick(object sender, RoutedEventArgs e)
        {
            UserManager.LogOut();
        }
    }
}
