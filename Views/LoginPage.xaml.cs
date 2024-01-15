using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using TicketToRideGUI.Logic;
using TicketToRideGUI.TicketToRideService;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page, IPlayersConnectCallback
    {
        private MainWindow _mainWindow;
        public LoginPage()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        private void btnLoginClick(object sender, RoutedEventArgs e)
        {
            try
            {
                User userPlayer = new User()
                {
                    Email = txbEmailLogin.Text,
                    Password = pwbPassword.Password
                };
                InstanceContext context = new InstanceContext(this);
                TicketToRideService.IUserPlayer user = new TicketToRideService.UserPlayerClient();
                TicketToRideService.IPlayersConnect player = new TicketToRideService.PlayersConnectClient(context);
                bool validationUser = user.AuthenticateUser(userPlayer);

                if (validationUser)
                {
                    player.ConnectToTheGame(userPlayer.Email);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.InvalidUser);
                }
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

        private void btnGoBackHomePageClick(object sender, RoutedEventArgs e)
        {
            HomePage homeView = new HomePage();
            this.NavigationService.Navigate(homeView);
        }

        private void btnRegistrationClick(object sender, RoutedEventArgs e)
        {
            RegistrationPage registrationView = new RegistrationPage();
            this.NavigationService.Navigate(registrationView);
        }

        private void Hyperlink_ChangePassword(object sender, RoutedEventArgs e)
        {
            ChangePasswordPage changePasswordPage = new ChangePasswordPage();
            this.NavigationService.Navigate(changePasswordPage);
        }

        public void ReceiveResponseConnectStatus()
        {
            try
            {
                UserReference userReference = UserReference.GetInstance();
                userReference.email = txbEmailLogin.Text;
                TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                userReference.gamerKey = player.GetGamerKey(userReference.email);
                _mainWindow.SetUserReference(userReference);
                LobbiePage lobbieView = new LobbiePage();
                lobbieView.SetOnlineFriends();
                this.NavigationService.Navigate(lobbieView);
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

    }
}
