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
using Labb3Databaser.Manager;

namespace Labb3Databaser.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
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
