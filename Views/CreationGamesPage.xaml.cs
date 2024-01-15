using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicketToRideGUI.Logic;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for CreationGamesPage.xaml
    /// </summary>
    public partial class CreationGamesPage : Page, LobbiesService.ILobbiesCallback
    {
        private UserReference _userReference;
        public CreationGamesPage()
        {
            InitializeComponent();
            _userReference = UserReference.GetInstance();

        }

        private void BackClick(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnJoinLobbie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string lobbieID = txbLobbieID.Text;
                InstanceContext context = new InstanceContext(this);
                LobbiesService.ILobbies lobbies = new LobbiesService.LobbiesClient(context);
                lobbies.ExistLobbie(lobbieID);
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

        private void btnCreateLobbie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbxQuota.SelectedItem != null)
                {
                    if (cbxQuota.SelectedItem is ComboBoxItem selectedItem &&
                        int.TryParse(selectedItem.Content.ToString(), out int capacity))
                    {
                        string gamerKey = _userReference.GetGamerKey();
                        TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                        string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                        InstanceContext context = new InstanceContext(this);
                        LobbiesService.ILobbies lobbies = new LobbiesService.LobbiesClient(context);
                        lobbies.CreateLobbie(gamerKey, gamerTag, capacity);
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.CapacityRoom);
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

        public void ConfirmationCreationLobbie(bool confirmation)
        {
            if (!confirmation)
            {
                MessageBox.Show(Properties.Resources.ErrorCreatingRoom);
            }

        }

        public void ReceiveLobbieID(string lobbieID)
        {
            LobbieReference lobbieReference = LobbieReference.GetInstance();
            lobbieReference.keyLobbie = lobbieID;
            GameRoomPage gameRoomView = new GameRoomPage();
            this.NavigationService.Navigate(gameRoomView);
        }

        public void JoinedSucessFull(bool joinedSuccessful)
        {
            if (joinedSuccessful)
            {
                LobbieReference lobbieReference = LobbieReference.GetInstance();
                lobbieReference.keyLobbie = txbLobbieID.Text;
                GameRoomPage gameRoomView = new GameRoomPage();
                this.NavigationService.Navigate(gameRoomView);
            }
            else
            {
                MessageBox.Show(Properties.Resources.ErrorEnteringRoom);
            }

        }

        public void ReceiveResponseExistLobbie(bool response)
        {
            try
            {
                if (response)
                {
                    string lobbieID = txbLobbieID.Text;
                    InstanceContext context = new InstanceContext(this);
                    LobbiesService.ILobbies lobbies = new LobbiesService.LobbiesClient(context);
                    lobbies.IsEndedLobbie(lobbieID);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.IncorrectLobbyPassword);
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

        public void ReceiveResponseIsEndedLobbie(bool response)
        {
            try
            {
                if (!response)
                {
                    string lobbieID = txbLobbieID.Text;
                    InstanceContext context = new InstanceContext(this);
                    LobbiesService.ILobbies lobbies = new LobbiesService.LobbiesClient(context);
                    lobbies.LobbieAreAvailable(lobbieID);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.RoomNotAvailable);
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

        public void ReceiveResponseLobbieAreAvailable(bool response)
        {
            try
            {
                if (response)
                {
                    string lobbieID = txbLobbieID.Text;
                    string gamerKey = _userReference.GetGamerKey();
                    TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                    string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                    InstanceContext context = new InstanceContext(this);
                    LobbiesService.ILobbies lobbies = new LobbiesService.LobbiesClient(context);
                    lobbies.JoinLobbie(lobbieID, gamerKey, gamerTag);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.FullRoom);
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
