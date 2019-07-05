using System.ComponentModel;

namespace PickUpSports.Models.ViewModel.GameController
{
    public class ViewGameViewModel
    {

        private string contactName;

        public int GameId { get; set; }

        public int? ContactId { get; set; }

        public int PickUpGameId { get; set; }
        
        [DisplayName("Venue:")]
        public string Venue { get; set; }

        [DisplayName("Sport:")]
        public string Sport { get; set; }

        [DisplayName("Status:")]
        public string Status { get; set; }

        [DisplayName("Start Date:")]
        public string StartDate { get; set; }

        [DisplayName("End Date:")]
        public string EndDate { get; set; }

        [DisplayName("Contact Name:")]
        public string ContactName
        {
            get
            {
                return contactName;
            }
            set
            {
                if (ContactId == null)
                {
                    contactName = "- User no longer exists -";
                }
                else
                {
                    contactName = value;
                }
            }
        }

        public bool IsVenueOwner { get; set; } = false;

        public bool IsAlreadyJoined { get; set; } = false;

        public bool IsCreatorOfGame { get; set; } = false;
    }
}