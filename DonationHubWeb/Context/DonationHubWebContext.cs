using DonationHubWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DonationHubWeb.Context
{
    public class DonationHubWebContext : IdentityDbContext
    {
        public DonationHubWebContext(DbContextOptions<DonationHubWebContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Institution> Institutions { get; set; }
    }
}
