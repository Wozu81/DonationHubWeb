using DonationHubWeb.Models;
using DonationHubWeb.Services.Interfaces;
using DonationHubWeb.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DonationHubWeb.Controllers
{
    public class HomeController : Controller
	{
		private readonly IDonationService _donationService;
		private readonly IInstitutionService _institutionService;
		public HomeController(IDonationService donationService, IInstitutionService institutionService)
		{
			_donationService = donationService;
			_institutionService = institutionService;
		}
		public IActionResult Index()
		{
			IndexViewModel indexViewModel = new IndexViewModel()
			{
				NumberOfAllDonations = _donationService.GetNumberOfAllDonations(),
				NumberOfSupportedInstitutions = _donationService.GetNumberOfSupportedInstitutions(),
				InstitutionsData = _institutionService.GetInstitutionsData()
			};
			return View(indexViewModel);
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}