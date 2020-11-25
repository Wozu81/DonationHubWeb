using DonationHubWeb.Context;
using DonationHubWeb.Models;
using DonationHubWeb.Services.Interfaces;
using DonationHubWeb.ViewModel.Donation;
using System.Collections.Generic;
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

        public int GetNumberOfSupportedInstitutions()
        {
            var a = _context.Donations.Select(i => i.Institution).Distinct().Count();
            return a;
        }

        public List<Category>  GetAllCategories()
        {
            var a = _context.Categories.ToList();
            return a;
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
        public List<CategoriesCheckboxModel> ConvertCategoriesToCategoriesForCheckBox(List<Category> categories)
        {
            List<CategoriesCheckboxModel> categoriesCheckboxes = new List<CategoriesCheckboxModel>();
            foreach (var category in categories)
            {
                categoriesCheckboxes.Add( new CategoriesCheckboxModel()
                {
                    Value = category.Id,
                    Name = category.Name,
                    IsChecked = false
                });
            }
         return categoriesCheckboxes;
        }
        public List<Category> ConvertCategoriesForCheckBoxToCategories(List<CategoriesCheckboxModel> categoriesCheckboxes)
        {
            List<Category> categories = new List<Category>();
            foreach (var categoryCheckBox in categoriesCheckboxes)
            {
                if (categoryCheckBox.IsChecked == true)
                {
                    categories.Add(new Category()
                    {
                        Id = categoryCheckBox.Value,
                        Name = categoryCheckBox.Name
                    });
                }
            }
            return categories;
        }
    }
}
