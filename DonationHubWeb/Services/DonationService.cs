using DonationHubWeb.Context;
using DonationHubWeb.Models;
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

        public bool Create(Donation donation)
        {
            _context.Donations.Add(donation);
            var a = _context.SaveChanges();
            return a > 0;
        }
        public int GetNumberOfAllDonations()
        {
            var a = _context.Donations.Select(d => d.Quantity).Sum();
            return a;
        }
    }
}
