using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ITimePreferenceRepository _timePreferenceRepository;
        private readonly ISportPreferenceRepository _sportPreferenceRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ISportRepository _sportRepository;
        private readonly IPickUpGameRepository _pickUpGameRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IFriendRepository _friendRepository;

        public ContactService(IContactRepository contactRepository, 
            ITimePreferenceRepository timePreferenceRepository, 
            ISportPreferenceRepository sportPreferenceRepository, 
            IReviewRepository reviewRepository, 
            ISportRepository sportRepository, 
            IPickUpGameRepository pickUpGameRepository, 
            IGameRepository gameRepository, 
            IFriendRepository friendRepository)
        {
            _contactRepository = contactRepository;
            _timePreferenceRepository = timePreferenceRepository;
            _sportPreferenceRepository = sportPreferenceRepository;
            _reviewRepository = reviewRepository;
            _sportRepository = sportRepository;
            _pickUpGameRepository = pickUpGameRepository;
            _gameRepository = gameRepository;
            _friendRepository = friendRepository;
        }

        /*
         * Given an email, return a Contact
         */
        public Contact GetContactByEmail(string email)
        {
            return _contactRepository.GetContactByEmail(email);
        }

        /*
         * Given a Contact ID, return a Contact
         */
        public Contact GetContactById(int? id)
        {
            return _contactRepository.GetContactById(id);
        }

        /*
         * Given a Username, check if that username is already taken 
         */
        public bool UsernameIsTaken(string username)
        {
            var existing = _contactRepository.GetContactByUsername(username);
            if (existing == null) return false;
            return true;
        }

        /*
         * Given a Username, return a Contact
         */
        public Contact GetContactByUsername(string username)
        {
            var contact = _contactRepository.GetContactByUsername(username);
            if (contact == null) return null;
            return contact;
        }

        /*
         * Create a new Contact entity
         */
        public Contact CreateContact(Contact contact)
        {
            return _contactRepository.CreateContact(contact);
        }

        /*
         * Edit an existing Contact entity
         */
        public void EditContact(Contact contact)
        {
            _contactRepository.EditContact(contact);
        }

        /*
         * Given a Contact, delete any preferences or games for that user
         * then delete them from Contact table
         */
        public void DeleteUser(Contact contact)
        {
            // Delete any SportPreferences related to Contact
            var sportPreferences = GetAllSportPreferences().Where(x => x.ContactID == contact.ContactId).ToList();
            if (sportPreferences.Count > 0)
            {
                foreach (var sportPreference in sportPreferences)
                {
                    _sportPreferenceRepository.Delete(sportPreference);
                }
            }

            // Delete any TimePreferences related to Contact
            var timePreferences = GetUserTimePreferences(contact.ContactId);
            if (timePreferences != null)
            {
                foreach (var timePreferece in timePreferences)
                {
                    _timePreferenceRepository.Delete(timePreferece);
                }
            }

            // Set ContactID for any Reviews to null
            var reviews = _reviewRepository.GetReviewsByContactId(contact.ContactId);

            if (reviews.Count > 0)
            {
                foreach (var review in reviews)
                {
                    review.ContactId = null;
                    _reviewRepository.EditReview(review);
                }
            }

            // Delete any pick up games that the user joined but did not create
            var pickUpGames = _pickUpGameRepository.GetPickUpGameListByContactId(contact.ContactId);

            if (pickUpGames != null)
            {
                foreach (var pickUpGame in pickUpGames)
                {
                    _pickUpGameRepository.DeletePickUpGame(pickUpGame);
                }
            }

            // Find any games that the user created. 
            // If no users in the games they created then delete them 
            // If there are joined users then set ContactID to null
            var allGames = _gameRepository.GetAllGames();
            var games = allGames.Where(x => x.ContactId == contact.ContactId);
            if (games != null)
            {
                foreach (var game in games)
                {
                    var players = _pickUpGameRepository.GetPickUpGameListByGameId(game.GameId);
                    if (players != null)
                    {
                        // There are people that joined game, set contact ID to null
                        game.ContactId = null;
                        _gameRepository.EditGame(game);
                    }
                    else
                    {
                        // No users, delete game
                        _gameRepository.DeleteGame(game);
                    }
                }
            }

            // Delete friends 
            var usersFriends = _friendRepository.GetContactsFriends(contact.ContactId);
            foreach (var usersFriend in usersFriends)
            {
                _friendRepository.Delete(usersFriend);
            }

            var friends = _friendRepository.GetFriends(contact.ContactId);
            foreach (var friend in friends)
            {
                _friendRepository.Delete(friend);
            }

            // Remove from Contact table
            _contactRepository.DeleteContact(contact);
        }

        /*
         * Return a list of all sport preferences for all users
         */
        public List<SportPreference> GetAllSportPreferences()
        {
            return _sportPreferenceRepository.GetAllSportsPreferences();
        }

        /*
         * Return a list of all time preferences for all users
         */
        public List<TimePreference> GetAllTimePreferences()
        {
            return _timePreferenceRepository.GetAllTimePreferences();
        }

        /*
         * Given a Contact ID, return that user's sports preferences
         */
        public List<string> GetUserSportPreferences(int contactId)
        {
            var sportPreferences = _sportPreferenceRepository.GetAllSportsPreferences();
            var userPrefs = from s in sportPreferences
                where s.ContactID == contactId
                select s;

            if (userPrefs.ToList().Count < 1) return null;

            var results = new List<string>();
            foreach (var sportPreference in userPrefs)
            {
                var sport = _sportRepository.GetSportById(sportPreference.SportID);
                results.Add(sport.SportName);
            }
            return results;
        }

        /*
         * Given a Contact ID, return that user's time preferences
         */
        public List<TimePreference> GetUserTimePreferences(int contactId)
        {
            var timePreferences = _timePreferenceRepository.GetAllTimePreferences();
            var userPrefs = timePreferences.Where(x => x.ContactID == contactId).ToList();

            if (userPrefs.Count == 0) return null;
            return userPrefs;
        }

        /*
         * Given a ContactID, return Contact's username
         */
        public string GetUsernameByContactId(int contactId)
        {
            var contact = _contactRepository.GetContactById(contactId);
            var username = "";
            if (contact == null) username = "- User no longer has account - ";
            else username = contact.Username;

            return username;
        }

        /*
         * Given an email, return true if an email is a valid email address
         */
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /*
         * Given a ContactID, return that Contact's friends
         */
        public List<Friend> GetUsersFriends(int contactId)
        {
            return _friendRepository.GetContactsFriends(contactId);
        }

        /*
         * Given a FriendID, get friends of that friend
         */
        public List<Friend> GetFriendsOfUser(int friendId)
        {
            return _friendRepository.GetFriends(friendId);
        }

        /*
         * Create a new Friend entity 
         */
        public Friend AddFriend(Friend friend)
        {
            return _friendRepository.AddFriend(friend);
        }
    }
}