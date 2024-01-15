using System.Collections.Generic;
using System.ComponentModel;
using TicketToRideGUI.TicketToRideService;

namespace TicketToRideGUI.Logic
{
    internal class LobbieViewFriends : INotifyPropertyChanged
    {
        private List<Friend> _friends;

        public List<Friend> Friends
        {
            get { return _friends; }
            set
            {
                _friends = value;
                OnPropertyChanged("ConectedFriends");
            }
        }

        public LobbieViewFriends()
        {
            Friends = new List<Friend>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
