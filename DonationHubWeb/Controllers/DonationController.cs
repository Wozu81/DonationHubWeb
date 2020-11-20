using DonationHubWeb.Services.Interfaces;
using DonationHubWeb.ViewModel.Donation;
using Microsoft.AspNetCore.Mvc;

namespace DonationHubWeb.Controllers
{
    public class DonationController : Controller
    {
        private readonly IDonationService _donationService;
        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Donate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Donate(DonateViewModel donateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = new DonateViewModel()
                    {
                        Categories = _donationService.GetAllCategories()
                    };
                    return View(model);
                }
                catch
                {
                    return View(donateViewModel);
                }
            }
            else
            {
                return View(donateViewModel);
            }
        }
    }
}
