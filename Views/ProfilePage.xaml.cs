using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicketToRideGUI.Logic;
using TicketToRideGUI.ProfileService;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page, ProfileService.IProfileCallback
    {
        private UserReference _userReference;
        public ProfilePage()
        {
            InitializeComponent();
            _userReference = UserReference.GetInstance();
            txbName.IsReadOnly = true;
            txbGamerTag.IsReadOnly = true;
            txbEmail.IsReadOnly = true;
            txbPlayerID.IsReadOnly = true;
            getData();

        }

        private void getData()
        {
            try
            {
                string gamerKey = _userReference.GetGamerKey();
                InstanceContext context = new InstanceContext(this);
                ProfileService.IProfile profile = new ProfileService.ProfileClient(context);
                profile.GetDataPlayer(gamerKey);
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

        public void ReceiveDataPlayer(Player player)
        {
            txbEmail.Text = player.Email;
            txbGamerTag.Text = player.GamerTag;
            txbName.Text = player.Name;
            txbPlayerID.Text = player.GamerKey;
        }

        private void BackClick(object sender, MouseButtonEventArgs e)
        {
            LobbiePage lobbieView = new LobbiePage();
            this.NavigationService.Navigate(lobbieView);
        }

        private void ChangeToScoreClick(object sender, MouseButtonEventArgs e)
        {
            ScorePage scoreView = new ScorePage();
            this.NavigationService.Navigate(scoreView);
        }
    }

}
