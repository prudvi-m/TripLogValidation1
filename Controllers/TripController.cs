using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using TripLog.Models;

namespace TripLog.Controllers
{
    public class TripController : Controller
    {
        private TripLogContext context { get; set; }
        public TripController(TripLogContext ctx) => context = ctx;
        public RedirectToActionResult Index() => RedirectToAction("Index", "Home");

        [HttpGet]
        public ViewResult Add(string id = "")
        {
            var vm = new TripViewModel();
            if (id.ToLower() == "page2")
            {
                // var contactInfoModel = new ConatactValidationModel();
                var accommodation = TempData[nameof(Trip.Accommodation)]?.ToString();
                if (string.IsNullOrEmpty(accommodation)) {  // skip to page 3
                    vm.PageNumber = 3;
                    var destination = TempData.Peek(nameof(Trip.Destination)).ToString();
                    vm.Trip = new Trip { Destination = destination };
                    return View("Add3", vm);
                }
                else {
                    var contactInfoModel = new ConatactValidationModel();
                    contactInfoModel.PageNumber = 2;
                    // vm.Trip = new Trip { Accommodation = accommodation };
                    // contactInfoModel.Accommodation = accommodation;
                    TempData.Keep(nameof(Trip.Accommodation));
            
                    ViewBag.SubHeader = $"Add Info for {TempData[nameof(Trip.Accommodation)]?.ToString()}";
                    return View("Add2", contactInfoModel);  
                }
            }  
            else if (id.ToLower() == "page3") 
            {
                vm.PageNumber = 3;
                vm.Trip = new Trip { Destination = TempData.Peek(nameof(Trip.Destination)).ToString() };
                return View("Add3", vm);
            }
            else
            {
                vm.PageNumber = 1;
                return View("Add1", vm);
            }      
        }

        [HttpPost]
        public IActionResult Add(TripViewModel vm)
        {
            if (vm.PageNumber == 1)
            {
                if (ModelState.IsValid)
                {
                    TempData[nameof(Trip.Destination)] = vm.Trip.Destination;
                    TempData[nameof(Trip.Accommodation)] = vm.Trip.Accommodation;
                    TempData[nameof(Trip.StartDate)] = vm.Trip.StartDate;
                    TempData[nameof(Trip.EndDate)] = vm.Trip.EndDate;
                    return RedirectToAction("Add", new { id = "Page2" });
                }
                else
                    return View("Add1", vm);
            }
            else if (vm.PageNumber == 2)
            {
                TempData[nameof(Trip.AccommodationPhone)] = vm.Trip.AccommodationPhone;
                TempData[nameof(Trip.AccommodationEmail)] = vm.Trip.AccommodationEmail;
                return RedirectToAction("Add", new { id = "Page3" });
            }
            else if (vm.PageNumber == 3)
            {
                vm.Trip.Destination = TempData[nameof(Trip.Destination)].ToString();
                vm.Trip.Accommodation = TempData[nameof(Trip.Accommodation)]?.ToString();
                vm.Trip.StartDate = (DateTime)TempData[nameof(Trip.StartDate)];
                vm.Trip.EndDate = (DateTime)TempData[nameof(Trip.EndDate)];
                vm.Trip.AccommodationPhone = TempData[nameof(Trip.AccommodationPhone)]?.ToString();
                vm.Trip.AccommodationEmail = TempData[nameof(Trip.AccommodationEmail)]?.ToString();

                context.Trips.Add(vm.Trip);
                context.SaveChanges();
                TempData["message"] = $"Trip to {vm.Trip.Destination} added.";
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public RedirectToActionResult Cancel()
        {
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AddContact(ConatactValidationModel vm) {
            
            if (ModelState.IsValid) {
                TempData[nameof(Trip.AccommodationPhone)] = vm.AccommodationPhone;
                TempData[nameof(Trip.AccommodationEmail)] = vm.AccommodationEmail;
                return RedirectToAction("Add", new { id = "Page3" });
            }
            ViewBag.SubHeader = $"Add Info for {TempData[nameof(Trip.Accommodation)]?.ToString()}";
            return View("Add2", vm);
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Index(Trip trip)
        {
            if (TempData["validEmail"] == null)
            {
                string msg = MailValidation.EmailDupplicateCheck(
                context, trip.AccommodationEmail);
                if (!String.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(
                    nameof(trip.AccommodationEmail), msg);
                }
            }
            if (ModelState.IsValid)
            {
                context.Trips.Add(trip);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(trip);
        }
    }
}