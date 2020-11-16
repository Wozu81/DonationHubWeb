using System;
using System.Collections.Generic;

namespace DonationHubWeb.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public List<Category> Categories { get; set; }
        public Institution Institution { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DateTime { get; set; }
        public string PickUpTime { get; set; }
    }
}
