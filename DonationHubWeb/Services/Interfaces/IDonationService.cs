
using System.Collections.Generic;

namespace DonationHubWeb.Services.Interfaces
{
    public interface IDonationService
    {
        int GetNumberOfAllDonations();
        int GetNumberOfSupportedInstitutions();
        IList<string> GetAllCategories();
    }
}
