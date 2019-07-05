using System.Collections.Generic;
using PickUpSports.Models.GoogleApiModels;

namespace PickUpSports.Interface
{
    public interface IPlacesApiClient
    {
        List<PlaceSearchResult> GetVenues();

        PlaceDetailsResponse GetPlaceDetailsById(string placeId);
    }
}
