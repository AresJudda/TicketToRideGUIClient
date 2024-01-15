using System;
using System.Windows;
using System.Windows.Controls;
using TicketToRideGUI.Logic;
using TicketToRideGUI.TicketToRideService;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {

        public RegistrationPage()
        {
            InitializeComponent();

        }

        private void btnLoginClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnRegisterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Operation operation = new Operation();

                Player userPlayer = new Player()
                {
                    Email = txbEmail.Text,
                    Password = pwbPassword.Password,
                    GamerTag = txbGamerTag.Text,
                    GamerKey = operation.GenerateSecureGamerKey(),
                    Reports = 0,
                    Name = txbName.Text

                };

                if (!VerifyPlayer())
                {
                    TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                    bool successfulRegistration = player.AddUser(userPlayer.Email, userPlayer.Password);

                    if (successfulRegistration)
                    {
                        AddPlayer(userPlayer);
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.ExistingMail);
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.ExistingGamertag);
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

        private bool VerifyPlayer()
        {
            String gamerTag = txbGamerTag.Text;
            TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
            bool succesfulVerification = player.IsPlayerExisting(gamerTag);
            return succesfulVerification;
        }

        private void AddPlayer(Player userPlayer)
        {
            try
            {
                TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                bool successfulRegistration = player.AddPlayer(userPlayer);

                if (successfulRegistration)
                {
                    MessageBox.Show(Properties.Resources.SuccessfullyRegisteredPlayer);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.PlayerNotRegistered);
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
    }
}



