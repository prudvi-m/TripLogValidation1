using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TripLog.Models;
using static System.Net.Mime.MediaTypeNames;
using TripLog.Controllers;

namespace TripLog.Controllers
{
    public class ValidationController : Controller
    {
        private TripLogContext context { get; set; }
        public JsonResult IsExisted(string emailAddress)
        {
            string errorMessage = MailValidation.EmailDupplicateCheck(context, emailAddress);
            if (string.IsNullOrEmpty(errorMessage))
            {
                TempData["validEmail"] = true;
                return Json(true);
            }
            return Json(errorMessage);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
