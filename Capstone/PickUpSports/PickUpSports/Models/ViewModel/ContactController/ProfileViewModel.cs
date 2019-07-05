namespace PickUpSports.Models.ViewModel.ContactController
{
    /**
     * Model for properties that will show on user's public profile
     */
    public class ProfileViewModel
    {
        public int ContactId { get; set; }

        public string Username { get; set; }

        public bool UserAllowsPublicProfile { get; set; }
    }
}