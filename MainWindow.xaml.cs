using System.ComponentModel;
using System.ServiceModel;
using System.Windows;
using TicketToRideGUI.Logic;
using TicketToRideGUI.Music;
using TicketToRideGUI.TicketToRideService;
using TicketToRideGUI.Views;


namespace TicketToRideGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPlayersConnectCallback
    {
        private UserReference _userReference;
        private MusicPlayer _musicPlayer;

        public MainWindow()
        {
            InitializeComponent();
            _musicPlayer = new MusicPlayer();
            string musicFileName = "Roundup on the Prairie.mp3";
            _musicPlayer.PlayMusic(musicFileName);
            MainFrame.NavigationService.Navigate(new HomePage());
        }

        public void ReceiveResponseConnectStatus()
        {
            this.Close();
        }

        public void SetUserReference(UserReference userReference)
        {
            _userReference = UserReference.GetInstance();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres cerrar la aplicación?", "Confirmar cierre", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                if (_userReference != null)
                {
                    InstanceContext context = new InstanceContext(this);
                    TicketToRideService.IPlayersConnect player = new TicketToRideService.PlayersConnectClient(context);
                    string userEmail = _userReference.GetEmail();
                    player.DisconnectFromTheGame(userEmail);
                    _musicPlayer.StopMusic();
                }
            }
        }

    }
}

