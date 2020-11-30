using DonationHubWeb.Models;

namespace DonationHubWeb.Services.Interfaces
{
    public interface IDonationService
    {
        bool Create(Donation donation);
        int GetNumberOfAllDonations();
    }
}
