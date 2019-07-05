using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.DAL;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel.ContactController;

namespace PickUpSports.Controllers
{
    [Authorize]
    public class TimePreferencesController : Controller
    {
        private readonly PickUpContext _context;

        public TimePreferencesController(PickUpContext context)
        {
            _context = context;
        }


        // GET: TimePreferences/Details/5
        public ActionResult Details(int? id)
        {
            //Identify the person using email
            string newContactEmail = User.Identity.GetUserName();

            //find the contact
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email == newContactEmail);


            //Send a list of time preferences back that match the contact id
            return View(_context.TimePreferences.Where(x => x.ContactID == contact.ContactId).OrderBy(x => x.DayOfWeek).ToList());
        }

        // GET: TimePreferences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimePreferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTimePreferenceViewModel model)
        {
            //Check the model state
            if (!ModelState.IsValid) return View(model);

            //Find the user to add the ID with
            string email = User.Identity.GetUserName();
            Contact contact = _context.Contacts.FirstOrDefault(c => c.Email == email);

            //Creating the TimePreference tieing in with contact using contact credentials (ContactID)
            TimePreference timePreference = new TimePreference()
            {
                ContactID = contact.ContactId,
                BeginTime = model.BeginTime,
                DayOfWeek = (int) model.DayOfWeek,
                EndTime = model.EndTime,
            };

            //Save to dB
            _context.TimePreferences.Add(timePreference);
            _context.SaveChanges();
            
             return RedirectToAction("Details", "TimePreferences");

        }

        // GET: TimePreferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TimePreference timePreference = _context.TimePreferences.Find(id);

            CreateTimePreferenceViewModel model = new CreateTimePreferenceViewModel()
            {
                BeginTime = timePreference.BeginTime,
                ContactID = timePreference.ContactID,
                DayOfWeek = (DayOfWeek) timePreference.DayOfWeek,
                EndTime = timePreference.EndTime,
                TimePrefID = timePreference.TimePrefID
            };

            if (timePreference == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: TimePreferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateTimePreferenceViewModel model)
        {
            //Get the user and match with the Time preference
            string email = User.Identity.GetUserName();
            Contact contact = _context.Contacts.FirstOrDefault(c => c.Email == email);


            //edit existing time preference
            TimePreference existing = new TimePreference()
            {
                ContactID = contact.ContactId,
                BeginTime = model.BeginTime,
                DayOfWeek = (int) model.DayOfWeek,
                EndTime = model.EndTime,
                TimePrefID = model.TimePrefID
                
            };

            //save changes
            _context.Entry(existing).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Details");

            
        }

        // GET: TimePreferences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimePreference timePreference = _context.TimePreferences.Find(id);
            if (timePreference == null)
            {
                return HttpNotFound();
            }
            return View(timePreference);
        }

        // POST: TimePreferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimePreference timePreference = _context.TimePreferences.Find(id);
            _context.TimePreferences.Remove(timePreference);
            _context.SaveChanges();
            return RedirectToAction("Details");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
