using DonationHubWeb.Context;
using DonationHubWeb.Services.Interfaces;
using System.Linq;

namespace DonationHubWeb.Services
{
    public class DonationService : IDonationService
    {
        private readonly DonationHubWebContext _context;

        public DonationService(DonationHubWebContext context)
        {
            _context = context;
        }

        public int GetNumberOfAllDonations()
        {
            var a = _context.Donations.Select(d => d.Quantity).Sum();
            return a;
        }

        public int GetNumberOfSupportedInstitutions()
        {
            var a = _context.Donations.Select(i => i.Institution).Distinct().Count();
            return a;
        }
    }
}
