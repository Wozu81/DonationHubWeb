using DonationHubWeb.Context;
using DonationHubWeb.Models;
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

        public int GetNumberOfSupportedInstitutions()
        {
            var a = _context.Donations.Select(i => i.InstitutionId).Distinct().Count();
            return a;
        }

        public Dictionary<string, string> GetInstitutionsData()
        {
            Dictionary<string, string> InstitutionsData = new Dictionary<string, string>();
            InstitutionsData = _context.Institutions.Select(t => new { t.Name, t.Description }).Distinct().ToDictionary(t => t.Name, t => t.Description );
            return InstitutionsData;
        }

        public List<Institution> GetAllInstitutions()
        {
            var a = _context.Institutions.ToList();
            return a;
        }

        public Institution GetInstitutionByID(int id)
        {
            var a = _context.Institutions.Where(i => i.Id == id).FirstOrDefault();
            return a;
        }
    }
} 
