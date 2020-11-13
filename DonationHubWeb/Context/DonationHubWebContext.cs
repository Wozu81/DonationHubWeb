using DonationHubWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
