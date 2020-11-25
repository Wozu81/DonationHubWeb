using DonationHubWeb.Models;
using DonationHubWeb.ViewModel.Donation;
using System.Collections.Generic;

namespace DonationHubWeb.Services.Interfaces
{
    public interface IDonationService
    {
        bool Create(Donation donation);
        int GetNumberOfAllDonations();
        int GetNumberOfSupportedInstitutions();
        List<Category> GetAllCategories();
        List<Institution> GetAllInstitutions();
        Institution GetInstitutionByID(int id);
        List<CategoriesCheckboxModel> ConvertCategoriesToCategoriesForCheckBox(List<Category> categories);
        List<Category> ConvertCategoriesForCheckBoxToCategories(List<CategoriesCheckboxModel> categoriesCheckboxes);
    }
}
