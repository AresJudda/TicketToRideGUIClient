using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for LostGame.xaml
    /// </summary>
    public partial class LostGame : Page
    {
        public LostGame()
        {
            InitializeComponent();
            string routeGif = "/Images/bugs.gif";
            var gifBitmap = new BitmapImage(new Uri(routeGif, UriKind.RelativeOrAbsolute));
            ImageBehavior.SetAnimatedSource(imgLostGame, gifBitmap);
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
        {
            LobbiePage lobbie = new LobbiePage();
            this.NavigationService.Navigate(lobbie);
        }
    }
}
