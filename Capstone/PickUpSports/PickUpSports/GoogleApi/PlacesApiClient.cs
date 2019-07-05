using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PickUpSports.Interface;
using PickUpSports.Models.GoogleApiModels;
using RestSharp;

namespace PickUpSports.GoogleApi
{
    public class PlacesApiClient : IPlacesApiClient
    {
        private static IRestClient _client;
        private readonly string _apiKey;

        public PlacesApiClient(IRestClient client, string apiKey)
        {
            _client = client;
            _apiKey = apiKey;
        }

        /**
         * Check Google API for different sports venues
         */
        public List<PlaceSearchResult> GetVenues()
        {
            List<GeometryLocation> cities = new List<GeometryLocation>();

            // Salem
            cities.Add(new GeometryLocation{Latitude = 44.9429, Longitude = -123.0351});

            // Independence
            cities.Add(new GeometryLocation { Latitude = 44.8512, Longitude = -123.1868 });

            //Monmouth
            cities.Add(new GeometryLocation { Latitude = 44.8485, Longitude = -123.2340 });

            // Initialize list to store all venue results
            List<PlaceSearchResult> results = new List<PlaceSearchResult>();

            foreach (var city in cities)
            {
                // Get basketball venues
                PlaceSearchResponse basketballVenues = GetPlaces("basketball", "park", city);
                foreach (var venue in basketballVenues.Results) results.Add(venue);

                // Get football venues
                PlaceSearchResponse footballVenues = GetPlaces("football", "school", city);
                foreach (var venue in footballVenues.Results) results.Add(venue);

                // Get tennis venues
                PlaceSearchResponse tennisVenues = GetPlaces("tennis", "park", city);
                foreach (var venue in tennisVenues.Results) results.Add(venue);

                // Get baseball venues
                PlaceSearchResponse baseballVenues = GetPlaces("baseball", "park", city);
                foreach (var venue in baseballVenues.Results) results.Add(venue);

                // Get softball venues
                PlaceSearchResponse softballVenues = GetPlaces("softball", "park", city);
                foreach (var venue in softballVenues.Results) results.Add(venue);

                // Get golf venues
                PlaceSearchResponse golfVenues = GetPlaces("golf", "park", city);
                foreach (var venue in golfVenues.Results) results.Add(venue);

                // Get volleyball venues
                PlaceSearchResponse volleyballVenues = GetPlaces("volleyball", "park", city);
                foreach (var venue in volleyballVenues.Results) results.Add(venue);
            }

            return results;
        }

        /**
         * Method that makes call to Google Places API and
         * retrieves list of places given specific parameters
         * <param name="keyword">Term to match against all content Google has indexed
         * including but not limited to name, type, and address</param>
         * <param name="type">Used to indicate status.</param>
         */
        public PlaceSearchResponse GetPlaces(string keyword, string type, GeometryLocation location)
        {
            // Initialize API request with URL and API key
            RestRequest request = new RestRequest("/nearbysearch/json?", Method.GET);
            request.AddQueryParameter("key", _apiKey);

            // Add parameters to API request, these will need to be changed
            request.AddQueryParameter("keyword", keyword);
            request.AddQueryParameter("location", $"{location.Latitude}, {location.Longitude}");
            request.AddQueryParameter("radius", "10000");
            request.AddQueryParameter("type", type);

            // Get respones from API using above request
            IRestResponse response = _client.Execute(request);

            // Convert JSON response to model and return response
            PlaceSearchResponse apiResponse = JsonConvert.DeserializeObject<PlaceSearchResponse>(response.Content);
            return apiResponse;
        }

        public PlaceDetailsResponse GetPlaceDetailsById(string placeId)
        {
            // Initialize API request with URL and API key
            RestRequest request = new RestRequest("/details/json?", Method.GET);
            request.AddQueryParameter("key", _apiKey);
            request.AddQueryParameter("placeid", placeId);

            // Only pull name, business hours, reviews, and address data
            request.AddQueryParameter("fields", "geometry,name,opening_hours,reviews,address_component,formatted_phone_number");

            // Get responses from API using above request
            IRestResponse response = _client.Execute(request);

            // Convert JSON response to model and return response
            PlaceDetailsResponse apiResponse = JsonConvert.DeserializeObject<PlaceDetailsResponse>(response.Content);
            return apiResponse;
        }
    }
}
