using System.Security.Cryptography;
using System.Text;

namespace TicketToRideGUI.Logic
{
    internal class Operation
    {
        public string GenerateSecureGamerKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder stringBuilder = new StringBuilder();

            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[8];
                randomGenerator.GetBytes(data);

                foreach (byte b in data)
                {
                    stringBuilder.Append(chars[b % chars.Length]);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
