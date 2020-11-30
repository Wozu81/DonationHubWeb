using System.ComponentModel.DataAnnotations;

namespace DonationHubWeb.ViewModel.Account
{
    public class PasswordViewModel
    {
        public string ID { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string RepeatPassword { get; set; }
    }
}
