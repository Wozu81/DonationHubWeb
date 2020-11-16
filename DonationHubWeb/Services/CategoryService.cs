using DonationHubWeb.Context;
using DonationHubWeb.Services.Interfaces;

namespace DonationHubWeb.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DonationHubWebContext _context;

        public CategoryService(DonationHubWebContext context)
        {
            _context = context;
        }
    }
}
