namespace TicketToRideGUI.Logic
{
    internal class LobbieData
    {
        public LobbieViewFriends LobbieViewFriends { get; set; }
        public LobbieViewGamers LobbieViewGamers { get; set; }

        public LobbieData()
        {
            LobbieViewFriends = new LobbieViewFriends();
            LobbieViewGamers = new LobbieViewGamers();
        }
    }
}
