using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePasswordPage : Page, EmailManagementService.IEmailManagementCallback
    {
        private String _uniqueKey;
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private void btnGoBackLogginPageClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnSendEmailClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = txbEmail.Text;
                TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                if (player.IsEmailExisting(email))
                {
                    InstanceContext context = new InstanceContext(this);
                    EmailManagementService.IEmailManagement client = new EmailManagementService.EmailManagementClient(context);
                    client.SendPasswordResetEmail(email);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.MailNonexistent);
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
        private void btnChangePasswordClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string key = txbKey.Text;
                string password = txbPassword.Text;
                string email = txbEmail.Text;
                if (key.Equals(_uniqueKey))
                {
                    InstanceContext context = new InstanceContext(this);
                    EmailManagementService.IEmailManagement client = new EmailManagementService.EmailManagementClient(context);
                    client.ChangePassword(email, password);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.VerifyCode);
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

        private void visibleElements()
        {
            btnConfirmChanges.Visibility = Visibility.Visible;
            txbKey.Visibility = Visibility.Visible;
            txtKey.Visibility = Visibility.Visible;
            txbPassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Visible;
            txtEmailChangePassword.Visibility = Visibility.Collapsed;
            txbEmail.Visibility = Visibility.Collapsed;
        }

        public void ReceiveConfirmationEmail(bool confirm, string uniqueKey)
        {
            if (confirm)
            {
                MessageBox.Show(Properties.Resources.RestorePassword);
                _uniqueKey = uniqueKey;
                visibleElements();
            }
            else
            {
                MessageBox.Show(Properties.Resources.MailSendingError);
            }
        }

        public void ReceiveConfirmationChangePassword(bool confirm)
        {
            if (confirm)
            {
                MessageBox.Show(Properties.Resources.NewPassword);
            }
            else
            {
                MessageBox.Show(Properties.Resources.FailedPassword);
            }
            this.NavigationService.GoBack();
        }
    }
}
