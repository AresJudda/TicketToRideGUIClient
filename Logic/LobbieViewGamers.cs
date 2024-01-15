using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TicketToRideGUI.Logic
{
    internal class LobbieViewGamers : INotifyPropertyChanged
    {
        private List<String> _players;

        public List<String> Players
        {
            get { return _players; }
            set
            {
                _players = value;
                OnPropertyChanged("Players");
            }
        }

        public LobbieViewGamers()
        {
            Players = new List<String>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
