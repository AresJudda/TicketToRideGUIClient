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
    /// Interaction logic for ScorePage.xaml
    /// </summary>
    public partial class ScorePage : Page, ProfileService.IStatCallback
    {
        private UserReference _userReference;
        public ScorePage()
        {
            InitializeComponent();
            _userReference = UserReference.GetInstance();
            txbGamesWon.IsReadOnly = true;
            txbGamesPlayed.IsReadOnly = true;
            txbWinningPercentage.IsReadOnly = true;
            getStat();
        }

        private void getStat()
        {
            try
            {
                string gamerKey = _userReference.GetGamerKey();
                InstanceContext context = new InstanceContext(this);
                ProfileService.IStat stat = new ProfileService.StatClient(context);
                stat.GetStatPlayer(gamerKey);
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

        public void ReceiveStatPlayer(Stat statPlayer)
        {
            txbGamesPlayed.Text = statPlayer.GamesPlayed.ToString();
            txbGamesWon.Text = statPlayer.GamesWon.ToString();

            if (statPlayer.GamesPlayed == 0)
            {
                txbWinningPercentage.Text = "0%";
            }
            else
            {
                int percentage = (int)((double)statPlayer.GamesWon / statPlayer.GamesPlayed * 100);
                txbWinningPercentage.Text = $"{percentage}%";
            }
        }

        private void BackClick(object sender, MouseButtonEventArgs e)
        {
            LobbiePage lobbieView = new LobbiePage();
            this.NavigationService.Navigate(lobbieView);
        }

        private void ChangeToProfileClick(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
