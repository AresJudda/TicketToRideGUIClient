using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TicketToRideGUI.Logic;
using TicketToRideGUI.ProfileService;
using WpfAnimatedGif;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for GameWonPage.xaml
    /// </summary>
    public partial class GameWonPage : Page, ProfileService.IProfileCallback
    {
        private UserReference _userReference;
        public GameWonPage()
        {
            InitializeComponent();
            _userReference = UserReference.GetInstance();
            string routeGif = "/Images/Western sheriff.gif";
            var gifBitmap = new BitmapImage(new Uri(routeGif, UriKind.RelativeOrAbsolute));
            ImageBehavior.SetAnimatedSource(imgGameWon, gifBitmap);
            txbWinningPlayer.IsReadOnly = true;
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
            txbWinningPlayer.Text = player.GamerTag;
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            LobbiePage lobbie = new LobbiePage();
            this.NavigationService.Navigate(lobbie);
        }
    }
}
