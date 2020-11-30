using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationHubWeb.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public List<Category> Categories { get; set; }
        [ForeignKey("InstitutionId")]
        public Institution Institution { get; set; }
        public int InstitutionId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime PickUpTime { get; set; }
        public string PickUpComment { get; set; }
    }
}
