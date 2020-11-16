using System.Collections.Generic;

namespace DonationHubWeb.Services.Interfaces
{
    public interface IInstitutionService
    {
        Dictionary<string, string> GetInstitutionsData();
    }
}
