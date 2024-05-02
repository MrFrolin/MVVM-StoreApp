using Labb3Databaser.Manager;
using System.Windows;
using Labb3Databaser.Views;

namespace Labb3Databaser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
        }

        private void UserManager_CurrentUserChanged()
        {
            

            if (UserManager.IsAdminLoggedIn && UserManager.LoggedIn && MainTabControl.SelectedItem == LoginTab)
            {
                AdminTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Visible;
                LoginTab.Visibility = Visibility.Collapsed;
                AdminTab.IsSelected = true;
                
            }
            else if (!UserManager.IsAdminLoggedIn && UserManager.LoggedIn)
            {
                ShopTab.Visibility = Visibility.Visible;
                AdminTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Collapsed;
                ShopTab.IsSelected = true;
            }
            else if (!UserManager.LoggedIn)
            {
                ShopTab.Visibility = Visibility.Collapsed;
                AdminTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Visible;
                LoginTab.IsSelected = true;
            }
            else if (UserManager.IsAdminLoggedIn && UserManager.LoggedIn && MainTabControl.SelectedItem != LoginTab)
            {
                var selectedview = MainTabControl.SelectedItem;
                AdminTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Visible;
                LoginTab.Visibility = Visibility.Collapsed;
                AdminTab.IsSelected = true;
                MainTabControl.SelectedItem = selectedview;
            }
        }

    }
}