using System.ComponentModel.DataAnnotations;

namespace DonationHubWeb.ViewModel.Account
{
    public class EditUserViewModel
    {
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Adres e-mail jest wymagany!")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
        public string Email { get; set; }
    }
}
