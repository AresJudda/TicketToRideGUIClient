using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TicketToRideGUI.Logic;
using TicketToRideGUI.TicketToRideService;

namespace TicketToRideGUI.Views
{
    /// <summary>
    /// Interaction logic for LobbiePage.xaml
    /// </summary>
    public partial class LobbiePage : Page, TicketToRideService.IGameServicesCallback
    {
        private UserReference _userReference;
        private LobbieData lobbieData;

        internal LobbiePage()
        {
            InitializeComponent();
            _userReference = UserReference.GetInstance();
            lobbieData = new LobbieData();
            DataContext = lobbieData;
        }

        private void SettingsClick(object sender, MouseButtonEventArgs e)
        {
            MenuPage menuView = new MenuPage();
            this.NavigationService.Navigate(menuView);
        }

        private void MessagesClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string userGamerKey = _userReference.gamerKey;
                InstanceContext context = new InstanceContext(this);
                TicketToRideService.IGameServices client = new TicketToRideService.GameServicesClient(context);
                client.CheckFriendsRequest(userGamerKey);

                if (lsvMesages.Visibility == Visibility.Collapsed)
                {
                    lsvMesages.Visibility = Visibility.Visible;
                }
                else
                {
                    lsvMesages.Visibility = Visibility.Collapsed;
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

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock textBlock = (TextBlock)sender;
                string gamerTag = textBlock.Text;

                string headerKey = "InviteFriend";
                string localizedHeader = Properties.Resources.InviteFriend;
                MessageBoxResult result = MessageBox.Show($"{localizedHeader} {gamerTag}?", $"{headerKey}", MessageBoxButton.YesNo, MessageBoxImage.Question);


                if (result == MessageBoxResult.Yes)
                {
                    TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                    string userGamerKey = _userReference.GetGamerKey();
                    string playerGamerKey = player.SearchGamerKey(gamerTag);
                    InstanceContext context = new InstanceContext(this);
                    TicketToRideService.IGameServices client = new TicketToRideService.GameServicesClient(context);
                    client.SendFriendRequest(userGamerKey, playerGamerKey);
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

        private void txtSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchTerm = txbSearch.Text;
                string userEmail = _userReference.GetEmail();
                InstanceContext context = new InstanceContext(this);
                TicketToRideService.IGameServices client = new TicketToRideService.GameServicesClient(context);
                client.SearchPlayers(searchTerm, userEmail);
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

        private void FriendsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                TicketToRideService.IGameServices client = new TicketToRideService.GameServicesClient(context);
                string userGamerKey = _userReference.GetGamerKey();
                client.CheckFriendsConnected(userGamerKey);

                if (lbxFriends.Visibility == Visibility.Collapsed)
                {
                    lbxFriends.Visibility = Visibility.Visible;
                }
                else
                {
                    lbxFriends.Visibility = Visibility.Collapsed;
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

        public void ReciveListPlayers(string[] players)
        {
            lobbieData.LobbieViewGamers.Players = new List<String>(players);
            lstBoxResults.ItemsSource = lobbieData.LobbieViewGamers.Players;
        }

        public void ReciveOnlineFriendList(Friend[] friends)
        {
            Dispatcher.Invoke(() =>
            {
                lobbieData.LobbieViewFriends.Friends = new List<Friend>(friends);
                lbxFriends.ItemsSource = lobbieData.LobbieViewFriends.Friends;
            });
        }

        public void ReciveConfirmationFriendRequest(bool response)
        {
            if (response)
            {
                MessageBox.Show(Properties.Resources.SuccessfulFriendshipRequest);
            }
            else
            {
                MessageBox.Show(Properties.Resources.RepeatedFriendshipRequest);
            }
        }

        public void ReciveListFriendRequest(string[] players)
        {
            lobbieData.LobbieViewGamers.Players = new List<String>(players);
            lsvMesages.ItemsSource = lobbieData.LobbieViewGamers.Players;
        }

        private void btnAcept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                if (button.DataContext is string gamerTag)
                {
                    String selectedGamerTag = gamerTag;
                    TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                    string userGamerKey = _userReference.GetGamerKey();
                    string playerGamerKey = player.SearchGamerKey(selectedGamerTag);
                    InstanceContext context = new InstanceContext(this);
                    TicketToRideService.IGameServices client = new TicketToRideService.GameServicesClient(context);
                    client.AcceptFriendRequest(userGamerKey, playerGamerKey);

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

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                if (button.DataContext is string gamerTag)
                {
                    String selectedGamerTag = gamerTag;
                    TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                    string userGamerKey = _userReference.GetGamerKey();
                    string playerGamerKey = player.SearchGamerKey(selectedGamerTag);
                    InstanceContext context = new InstanceContext(this);
                    TicketToRideService.IGameServices client = new TicketToRideService.GameServicesClient(context);
                    client.RejectFriendRequest(userGamerKey, playerGamerKey);

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

        public void ReciveResponseFriendRequest(bool response)
        {
            try
            {
                if (response)
                {
                    string userEmail = _userReference.GetEmail();
                    string userGamerKey = _userReference.GetGamerKey();
                    InstanceContext context = new InstanceContext(this);
                    TicketToRideService.IGameServices client = new TicketToRideService.GameServicesClient(context);
                    client.CheckFriendsRequest(userGamerKey);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.FailedFriendshipRequest);
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

        private void btnPlayGame_Click(object sender, RoutedEventArgs e)
        {
            CreationGamesPage creationGamesPage = new CreationGamesPage();
            this.NavigationService.Navigate(creationGamesPage);
        }

        public void RecivePlayerDisconnected(string disconnectedGamerKey)
        {
            lobbieData.LobbieViewGamers.Players.Remove(disconnectedGamerKey);
            lbxFriends.ItemsSource = lobbieData.LobbieViewGamers.Players;
        }

        public void SetOnlineFriends()
        {
            try
            {
                string userGamerKey = _userReference.GetGamerKey();
                InstanceContext context = new InstanceContext(this);
                TicketToRideService.IGameServices client = new TicketToRideService.GameServicesClient(context);
                client.CheckFriendsConnect(userGamerKey);
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
