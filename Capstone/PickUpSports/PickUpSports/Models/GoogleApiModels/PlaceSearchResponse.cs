using System.Collections.Generic;
using Newtonsoft.Json;

namespace PickUpSports.Models.GoogleApiModels
{
    public class PlaceSearchResponse
    {
        [JsonProperty("html_attributions")]
        public List<object> HtmlAttributions { get; set; }

        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }

        [JsonProperty("results")]
        public List<PlaceSearchResult> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class PlaceSearchResult
    {
        [JsonProperty("geometry")]
        public PlaceSearchGeometry Geometry { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("alt_ids")]
        public List<AltId> AltIds { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }

        [JsonProperty("vicinity")]
        public string Vicinity { get; set; }
    }

    public class PlaceSearchGeometry
    {
        [JsonProperty("location")]
        public PlaceSearchLocation Location { get; set; }
    }

    public class PlaceSearchLocation
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }

    public class AltId
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
