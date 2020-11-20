using DonationHubWeb.Context;
using DonationHubWeb.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DonationHubWeb.Services
{
    public class InstitutionService : IInstitutionService
    {
        private readonly DonationHubWebContext _context;

        public InstitutionService(DonationHubWebContext context)
        {
            _context = context;
        }

        public Dictionary<string, string> GetInstitutionsData()
        {
            Dictionary<string, string> InstitutionsData = new Dictionary<string, string>();
            InstitutionsData = _context.Institutions.Select(t => new { t.Name, t.Description }).ToDictionary(t => t.Name, t => t.Description );
            return InstitutionsData;
        }
    }
} 
