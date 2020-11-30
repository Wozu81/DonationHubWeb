using DonationHubWeb.Context;
using DonationHubWeb.Models;
using DonationHubWeb.Services.Interfaces;
using DonationHubWeb.ViewModel.Donation;
using System.Collections.Generic;
using System.Linq;

namespace DonationHubWeb.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DonationHubWebContext _context;

        public CategoryService(DonationHubWebContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            var a = _context.Categories.AsEnumerable().GroupBy(n => n.Name).Select(f => f.First()).ToList();
            return a;
        }

        public List<CategoriesCheckboxModel> ConvertCategoriesToCategoriesForCheckBox(List<Category> categories)
        {
            List<CategoriesCheckboxModel> categoriesCheckboxes = new List<CategoriesCheckboxModel>();
            foreach (var category in categories)
            {
                categoriesCheckboxes.Add(new CategoriesCheckboxModel()
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
                        Id = 0,
                        Name = categoryCheckBox.Name
                    });
                }
            }
            return categories;
        }
    }
}
