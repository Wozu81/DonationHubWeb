using DonationHubWeb.Models;
using DonationHubWeb.Services.Interfaces;
using DonationHubWeb.ViewModel.Donation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace DonationHubWeb.Controllers
{
    public class DonationController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly ICategoryService _categoryService;
        private readonly IInstitutionService _institutionService;
        public DonationController(IDonationService donationService, ICategoryService categoryService, IInstitutionService institutionService)
        {
            _donationService = donationService;
            _categoryService = categoryService;
            _institutionService = institutionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Donate()
        {
            var tempCategories = _categoryService.GetAllCategories();   //benchmarkowe zmienne, by sprawdzić czy listy po drodze się nie zepsuły
                                                                        //- tutaj przypisuję listę kategorii pobraną z bazy danych w klasie List<Category>
            var tempCategoriesForCheckBox = _categoryService.ConvertCategoriesToCategoriesForCheckBox(tempCategories);  //ponieważ po odebraniu informacji z checkboxów muszę mieć informację,
                                                                                                                        //która kategoria została zaznaczona, więc wprowadzam nową klasę 'CategoriesCheckboxModel',
                                                                                                                        //ma te same właściwości co Category, do tego właściwość boolowską 'IsChecked' przechowującą
                                                                                                                        //informację czy checkbox przy danej kategorii został zaznaczony.
                                                                                                                        //Metoda ConvertToCategoriesForCheckbox(List<Category>) tworzy obiekt klasy
                                                                                                                        //List<CategoriesCheckboxModel> na podstawie List<Category>
                                                                                                                        //Jest to potrzebne, bo wygenerowanie checkboxów przy użyciu TagHelpera '@Html.CheckBoxFor'
                                                                                                                        //binduje go do właściwości 'IsChecked'
            var model = new DonateViewModel()   //Model z danymi wyłanymi do wyświetlenia się na stronie.                                               
            {
                Categories = tempCategories,                            // W tej chwili nadmiarową właściwością jest wymagany przez projekt 'Categories'
                                                                        //'Categories' miałby być użyty w pętli foreach do generowania checkboxów. 
                CategoriesForCheckBox = tempCategoriesForCheckBox,      //Po testach używam 'CategoriesForCheckBox' w pętli for - w Donate.cshtml
                Institutions = _institutionService.GetAllInstitutions()    //Institutions - wymagany jako opis przy radiobuttonach przy wyborze fundacji
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Donate(DonateViewModel donateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var a = donateViewModel.CategoriesForCheckBox;      //zmienna w której sprawdzałem co znajduje się w pobranych kategoriach, w tym miejscu sprawdzałem, że foreach daje pustą listę
                    var b = _categoryService.ConvertCategoriesForCheckBoxToCategories(donateViewModel.CategoriesForCheckBox);   //pomocnicza zmienna do procesu zamiany List<CategoriesCheckboxModel> na List<Category>
                                                                                                                                //w wyniku tej funkcji do List<Category> wpadają tylko zaznaczone w formularzu instytucje
                    var model = new Donation()                                                                                  //przygotowanie modelu bez ID rzecz jasna - to mogłoby być już teoretycznie zapisane
                                                                                                                                //w bazie danych, ale po drodze jeszcze czeka wyświetlenie podsumowania i akceptacja
                    {
                        Categories = _categoryService.ConvertCategoriesForCheckBoxToCategories(donateViewModel.CategoriesForCheckBox),
                        Quantity = donateViewModel.Quantity,
                        Institution = _institutionService.GetInstitutionByID(donateViewModel.SelectedInstitution),
                        Street = donateViewModel.Street,
                        City = donateViewModel.City,
                        ZipCode = donateViewModel.ZipCode,
                        Phone = donateViewModel.Phone,
                        PickUpDate = donateViewModel.PickUpDate,
                        PickUpTime = donateViewModel.PickUpTime,
                        PickUpComment =donateViewModel.PickUpComment
                    };
                    //TempData["ForDonationSummary"] = JsonSerializer.Serialize(model);                     //--!--tymczasowa zmienna do przetrzymania Jsona z gotowym modelem
                    HttpContext.Session.SetString("ForDonationSummary", JsonSerializer.Serialize(model));   //Nie ważny TempData = wprowadziłem sesję, na razie wygasza po 90 sekundach (ustawione w startup.cs)
                    return RedirectToAction(nameof(DonateSummary));                                         //przekierowani na stronę z podsumowaniem
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

        [HttpGet]
        public IActionResult DonateSummary()
        {
            DonateSummaryViewModel donateSummaryView = JsonSerializer.Deserialize<DonateSummaryViewModel>((string)HttpContext.Session.GetString("ForDonationSummary")); //rozwinięcie Jsona do obiektu klasy DonateViewModel
            //DonateSummaryViewModel donateSummaryView = JsonSerializer.Deserialize<DonateSummaryViewModel>((string)TempData["ForDonationSummary"]);
            //TempData["ForDonationSummary"] = JsonSerializer.Serialize(donateSummaryView);     // --!-- Znów serializacja do zmiennej tymczasowej --!-- To jest pierwszy powód, dla którego zainteresowałem się sesją
                                                                                                // --!-- TempData trzeba po każdym "rozładowaniu" "ładować" ponownie. Sesja ma timeout lub zwlania się metodą Remove()
            return View(donateSummaryView);
        }

        [HttpPost]
        public IActionResult DonateSummary(DonateSummaryViewModel donateSummaryView)        //to... droga na skróty, akcja nie odbiera nic ze strony, gdyż DonateSummary.cshtml nie zawiera formularza
                                                                                            //po kliknięciu na submit, po prostu wykonuje się kod poniżej
        {
            donateSummaryView = JsonSerializer.Deserialize<DonateSummaryViewModel>((string)HttpContext.Session.GetString("ForDonationSummary"));    //To jest drugi powód użycia sesji w tak przygotowanym rozwiązaniu
                                                                                                                                                    //Jeśli nastąpi błąd zapisu do bazy danych, to w myśl logiki catch
                                                                                                                                                    //powinna wywołaś się akcja DonateSummary z przyjętym modelem,
                                                                                                                                                    //z tym, że jak wcześniej napisałem, model przyjmowany ze strony jest
                                                                                                                                                    //pusty. TempData, po kolejnym przebiegu - jest pusty. Stąd pomysł na
                                                                                                                                                    //sesję. Co wiąże się też z obsługą błędu Timeoutu, którego teraz nie ma.
                                                                                                                                                    //W tym momencie skutkiem timeoutu jest za którymś razem pobranie
                                                                                                                                                    //pustego modelu, tak jak pobranie modelu po raz drugi z TempData
            //donateSummaryView = JsonSerializer.Deserialize<DonateSummaryViewModel>((string)TempData["ForDonationSummary"]);

            if (ModelState.IsValid)
            {
                try
                {
                    var model = new Donation()
                    {
                        Categories = donateSummaryView.Categories,
                        Quantity = donateSummaryView.Quantity,
                        //Institution = donateSummaryView.Institution,
                        InstitutionId = donateSummaryView.Institution.Id,
                        Street = donateSummaryView.Street,
                        City = donateSummaryView.City,
                        ZipCode = donateSummaryView.ZipCode,
                        Phone = donateSummaryView.Phone,
                        PickUpDate = donateSummaryView.PickUpDate,
                        PickUpTime = donateSummaryView.PickUpTime,
                        PickUpComment = donateSummaryView.PickUpComment
                    };
                    //model.Institution.Id = 0;
                    //foreach (var category in model.Categories)
                    //{
                    //    category.Id = 0;
                    //}
                    _donationService.Create(model);                         
                    HttpContext.Session.Remove("ForDonationSummary");       //Wyrzucenie Sesji, jeśli zapis skończy się sukcesem i jeśli nie nastąpił timeout rzecz jasna
                    return RedirectToAction(nameof(DonateConfirmation));
                }
                catch (Exception e)
                {
                    throw e; 
                                        //Pojawia się błąd zapisu do tablicy Institutions...
                                        //W sumie, do czego doszedłem analizując podane w zadaniu modele i bazę SQL,
                                        //to to, że nie mam pojęcia w takim razie jak działają w ten sposób zapisane
                                        //modele bez np. kluczów obcych.
                                        //Migracja je tworzy w bazie i w MSSQL wygląda to chyba tak jak powinno (łącznie z kluczami obcymi),
                                        //ale modele tutaj ich nie posiadają, więc to co zrobiłem,
                                        //to pobrałem informacje modeli i w tym samym formacie (klasa do klasy) chciałem je zwrócić.
                                        //Coś czuję, że tutaj jest problem.
                }
            }
            else
            {
                return View(donateSummaryView);
            }

        }

        [HttpGet]
        public IActionResult DonateConfirmation()
        {
            return View();
        }
    }
}
