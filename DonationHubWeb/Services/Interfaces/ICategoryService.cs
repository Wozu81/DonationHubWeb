using DonationHubWeb.Models;
using DonationHubWeb.ViewModel.Donation;
using System.Collections.Generic;

namespace DonationHubWeb.Services.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        List<CategoriesCheckboxModel> ConvertCategoriesToCategoriesForCheckBox(List<Category> categories);
        List<Category> ConvertCategoriesForCheckBoxToCategories(List<CategoriesCheckboxModel> categoriesCheckboxes);
    }
}
