using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using TicketToRideGUI.Logic;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            string routeGif = "/Images/HomePageGif.gif";
            var gifBitmap = new BitmapImage(new Uri(routeGif, UriKind.RelativeOrAbsolute));
            ImageBehavior.SetAnimatedSource(gifImage, gifBitmap);
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMinimizeClick(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow((DependencyObject)sender);

            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void cmbSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLanguage.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedLanguage = selectedItem.Tag.ToString();
                Thread.CurrentThread.CurrentCulture = new CultureInfo(selectedLanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
                UpdateUIResourses();
                ChangeLanguage.OnLanguajeChanged();
            }
        }

        private void UpdateUIResourses()
        {
            Button btnLoggin = btnGoToLogin;
            btnLoggin.Content = Properties.Resources.btnGoToLogin;
        }

        private void btnGoToLoginClick(object sender, RoutedEventArgs e)
        {
            LoginPage loginView = new LoginPage();
            loginView.SetMainWindow(Window.GetWindow(this) as MainWindow);
            this.NavigationService.Navigate(loginView);
        }

    }
}
