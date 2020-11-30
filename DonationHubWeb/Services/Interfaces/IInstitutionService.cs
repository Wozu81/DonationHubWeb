using DonationHubWeb.Models;
using System.Collections.Generic;

namespace DonationHubWeb.Services.Interfaces
{
    public interface IInstitutionService
    {
        int GetNumberOfSupportedInstitutions();
        Dictionary<string, string> GetInstitutionsData();
        List<Institution> GetAllInstitutions();
        Institution GetInstitutionByID(int id);
    }
}
