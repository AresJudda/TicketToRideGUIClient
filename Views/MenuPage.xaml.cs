using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using TicketToRideGUI.Logic;
using TicketToRideGUI.TicketToRideService;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page, IPlayersConnectCallback
    {
        private UserReference _userReference;
        public MenuPage()
        {
            InitializeComponent();
            _userReference = UserReference.GetInstance();
        }

        private void btnProfileClick(object sender, RoutedEventArgs e)
        {
            ProfilePage profileView = new ProfilePage();
            this.NavigationService.Navigate(profileView);
        }

        private void btnReturnMenuClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnLogOutClick(object sender, RoutedEventArgs e)
        {
            DisconnectPlayer();
        }

        private void DisconnectPlayer()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                TicketToRideService.IPlayersConnect player = new TicketToRideService.PlayersConnectClient(context);
                string userEmail = _userReference.GetEmail();
                player.DisconnectFromTheGame(userEmail);
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
        }

        public void ReceiveResponseConnectStatus()
        {
            LoginPage loginView = new LoginPage();
            this.NavigationService.Navigate(loginView);
        }
    }
}
