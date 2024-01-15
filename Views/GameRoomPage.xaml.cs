using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicketToRideGUI.Logic;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for GameRoom.xaml
    /// </summary>
    public partial class GameRoomPage : Page, LobbiesService.IGameRoomCallback
    {
        private UserReference _userReference;
        private LobbieReference _lobbieReference;
        private LobbieData lobbieData;
        public GameRoomPage()
        {
            InitializeComponent();
            _lobbieReference = LobbieReference.GetInstance();
            txbKeyLobbie.Text = _lobbieReference.GetKeylobbie();
            lobbieData = new LobbieData();
            DataContext = lobbieData;
            IsLobbieHost();
        }

        private void IsLobbieHost()
        {
            try
            {
                String lobbieID = _lobbieReference.GetKeylobbie();
                _userReference = UserReference.GetInstance();
                string gamerKey = _userReference.GetGamerKey();
                InstanceContext context = new InstanceContext(this);
                LobbiesService.IGameRoom game = new LobbiesService.GameRoomClient(context);
                game.IsLobbieHost(gamerKey, lobbieID);
                CheckListFriends();
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

        private void CheckListFriends()
        {
            try
            {
                _userReference = UserReference.GetInstance();
                string gamerKey = _userReference.GetGamerKey();
                InstanceContext context = new InstanceContext(this);
                LobbiesService.IGameRoom game = new LobbiesService.GameRoomClient(context);
                game.GetFriendList(gamerKey);
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

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> playerNames = lobbieData.LobbieViewGamers.Players;
                string[] playerNamesArray = playerNames.ToArray();
                lstBoxFriends.ItemsSource = playerNames;
                InstanceContext context = new InstanceContext(this);
                LobbiesService.IGameRoom game = new LobbiesService.GameRoomClient(context);
                game.GoToTheGame(playerNamesArray);
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

        private void BackClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                String lobbieID = _lobbieReference.GetKeylobbie();
                string gamerKey = _userReference.GetGamerKey();
                TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                bool isHost = _lobbieReference.GetIsHost();
                InstanceContext context = new InstanceContext(this);
                LobbiesService.IGameRoom game = new LobbiesService.GameRoomClient(context);
                game.ExitFromLobbie(lobbieID, gamerKey, gamerTag, isHost);
                this.NavigationService.GoBack();
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

        public void ReceiveResponseIsLobbieHost(bool isLobbieHost)
        {
            if (isLobbieHost)
            {
                btnStartGame.Visibility = Visibility.Visible;
            }
            _lobbieReference = LobbieReference.GetInstance();
            _lobbieReference.isHost = isLobbieHost;

        }

        public void UpdatePlayerListRealTime(string[] playerList)
        {
            Dispatcher.Invoke(() =>
            {
                lobbieData.LobbieViewGamers.Players = new List<string>(playerList);
                lstGamers.ItemsSource = lobbieData.LobbieViewGamers.Players;
            });
        }
        public void ReceiveResponseExitFromLobbie(bool response)
        {
            if (response)
            {
                this.NavigationService.GoBack();
                MessageBox.Show(Properties.Resources.HostAbandonedRoom);
            }
        }

        public void ReceiveFriendList(string[] friendList)
        {
            lobbieData.LobbieViewGamers.Players = new List<string>(friendList);
            lstBoxFriends.ItemsSource = lobbieData.LobbieViewGamers.Players;
        }

        private void TextBlock_InvitateToGame(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock textBlock = (TextBlock)sender;
                string gamerTagFriend = textBlock.Text;
                TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                string LobbieID = txbKeyLobbie.Text;

                string headerKey = "InviteFriend";
                string localizedHeader = Properties.Resources.InviteFriend;
                MessageBoxResult result = MessageBox.Show($"{localizedHeader} {gamerTag}?", $"{headerKey}", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    InstanceContext context = new InstanceContext(this);
                    LobbiesService.IGameRoom client = new LobbiesService.GameRoomClient(context);
                    client.Invitatefriend(gamerTag, gamerTagFriend, LobbieID);

                }
            }
            catch (InvalidCastException)
            {
                MessageBox.Show(Properties.Resources.EmptyBoxes);
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

        public void ReceiveConfirmationOfInvitation(bool confirmationOfInvitation)
        {
            if (confirmationOfInvitation)
            {
                MessageBox.Show(Properties.Resources.PlayerGuest);
            }
        }

        public void ReceiveResponseGoToTheGame()
        {
            _lobbieReference = LobbieReference.GetInstance();
            _lobbieReference.players = lobbieData.LobbieViewGamers.Players;
            BoardPage gameBoardView = new BoardPage();
            this.NavigationService.Navigate(gameBoardView);
        }
    }
}
