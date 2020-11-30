using DonationHubWeb.Models;
using System;
using System.Collections.Generic;

namespace DonationHubWeb.ViewModel.Donation
{
    public class DonateViewModel
    {
        public List<Category> Categories { get; set; }
        public List<CategoriesCheckboxModel> CategoriesForCheckBox { get; set; }
        public int Quantity { get; set; }
        public List<Institution> Institutions { get; set; }
        public Institution Institution { get; set; }
        public int SelectedInstitution { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime PickUpTime { get; set; }
        public string PickUpComment { get; set; }
    }
}
