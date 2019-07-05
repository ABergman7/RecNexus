using System.Net.Mail;
using System.Web.Mvc;
using PickUpSports.Interface;
using PickUpSports.Models.ViewModel.HomeController;

namespace PickUpSports.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGMailService _gmailer;

        public HomeController(IGMailService gmailer)
        {
            _gmailer = gmailer;
        }

        [RequireHttps]
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Contact(ContactUsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubmitEmailSent = false;
                return View("AboutUs");
            }

            // Send email to Scrum Lords for approval
            var email = new MailMessage();
            email.To.Add(_gmailer.GetEmailAddress());
            email.From = new MailAddress(model.Email);
            email.Subject = $"Support inquiry received from {model.Name}.";
            email.Body = $"Received following inquiry from {model.Name}, email address {model.Email}: " + model.Message;
            email.IsBodyHtml = true;

            // Send email and return to view if successful
            if (_gmailer.Send(email))
            {
                ViewBag.SubmitEmailSent = true;
                return View("AboutUs", model);
            }

            // Email could not be sent, display error 
            ViewBag.SubmitEmailSent = false;
            ViewData.ModelState.AddModelError("EmailError", "Unfortunately, your email could not be sent.");
            return View("AboutUs", model);
        }
    } 
}