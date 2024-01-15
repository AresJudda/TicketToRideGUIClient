namespace TicketToRideGUI.Logic
{
    public class UserReference
    {
        private static UserReference instanceUser;
        public string email { get; set; }
        public string gamerKey { get; set; }

        private UserReference()
        {
        }

        public static UserReference GetInstance()
        {
            if (instanceUser == null)
            {
                instanceUser = new UserReference();
            }
            return instanceUser;
        }

        public string GetEmail()
        {
            return email;
        }

        public string GetGamerKey()
        {
            return gamerKey;
        }


    }
}
