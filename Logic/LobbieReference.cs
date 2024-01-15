using System.Collections.Generic;

namespace TicketToRideGUI.Logic
{
    internal class LobbieReference
    {
        private static LobbieReference instanceLobbie;
        public string keyLobbie { get; set; }
        public bool isHost { get; set; }
        public List<string> players { get; set; }

        private LobbieReference()
        {
        }

        public static LobbieReference GetInstance()
        {
            if (instanceLobbie == null)
            {
                instanceLobbie = new LobbieReference();
            }
            return instanceLobbie;
        }

        public string GetKeylobbie()
        {
            return keyLobbie;
        }

        public bool GetIsHost()
        {
            return isHost;
        }

        public List<string> GetPlayers()
        {
            return players;
        }
    }
}
