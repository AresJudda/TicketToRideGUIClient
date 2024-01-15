using System.Globalization;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using TicketToRideGUI.TicketToRideService;

namespace TicketToRideGUI.Validations
{
    public class EmailDuplicateValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            User user = new User();
            {
                user.Email = value.ToString();
            };

            try
            {
                TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                bool resultValidateEmail = player.IsEmailExisting(user.Email);

                if (!resultValidateEmail)
                {
                    return new ValidationResult(false, Properties.Resources.ExistingMail);
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
            catch (CommunicationException ex)
            {
                MessageBox.Show(ex.Message);
                return new ValidationResult(false, Properties.Resources.ConnectionError);
            }
        }
    }
}
