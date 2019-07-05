using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.TrendingController;
using PickUpSports.Services;
using static System.Int32;

namespace PickUpSports.Controllers
{
    public class TrendingController : Controller
    {
        private readonly PickUpContext _context;
        private readonly TrendingWebServices _trendingWebServices;
        private readonly IGameService _gameService;
        private readonly IVenueService _venueService;


        public TrendingController(PickUpContext context)
        {
            _context = context;
        }

        public TrendingController(IGameService gameService, IVenueService venueService)
        {
            
            _gameService = gameService;
           _venueService = venueService;
           _trendingWebServices = new TrendingWebServices();
        }

      public TrendingController()
        {
            _trendingWebServices = new TrendingWebServices();
        }


        public TrendingModel TrendingResult { get; set; }


        // GET: Trending
        public ActionResult Index()
        {
            PopulateDropdownValues();
            return View();
        }

      
        [HttpPost]
        public ActionResult GetPredictionFromWebService()
        {
            // var venueName = Request.Form["venueName"];
            
            //If the request is empty return error
            if (Request.Form["SportName"] == "")
            {
                ViewData.ModelState.AddModelError("NoSport","you have not selected any sport!");
                return RedirectToAction("Index");
            }
            int sportId = Parse(Request.Form["SportName"]);

            
            var sportName = _gameService.GetSportNameById(sportId);

            //Pass the sport name to the Machine Learning Class
            if (!string.IsNullOrEmpty(sportName))
            {
                var resultResponse = _trendingWebServices.InvokeRequestResponseService<ResultOutcome>(sportName).Result;
                if (resultResponse != null)
                {
                    var result = resultResponse.Results.Output1.Value.Values;
                    TrendingResult = new TrendingModel()
                    {
                        VenueName = result[0, 17],
                        SportName = result[0, 1]
                    };

                }
            }

            // store the returned Venue Name 
            string venName = TrendingResult.VenueName;

            // Find the venue by Name and return the ID
            var findVenue = _venueService.GetAllVenues().Where(v => v.Name == venName);
            int venID = findVenue.ElementAt(0).VenueId;

            //pass Venue Name and ID to View model
            ViewBag.myData = venName;
            ViewBag.venID = venID;
            PopulateDropdownValues();
            return View("Index");

        }

        //Populate the drop down list with all available sports
        public void PopulateDropdownValues()
        {
            ViewBag.Sport = _gameService.GetAllSports().ToList().ToDictionary(s => s.SportID, s => s.SportName);
        }
    }
}