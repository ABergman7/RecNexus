using System;
using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly PickUpContext _context;

        public FriendRepository(PickUpContext context)
        {
            _context = context;
        }

        public Friend AddFriend(Friend friend)
        {
            _context.Friends.Add(friend);
            _context.SaveChanges();
            return friend;
        }

        public void Delete(Friend friend)
        {
            if (friend == null) throw new ArgumentNullException();

            _context.Friends.Remove(friend);
            _context.SaveChanges();
        }

        public List<Friend> GetContactsFriends(int contactId)
        {
            var friends = _context.Friends.Where(x => x.ContactID == contactId).ToList();
            return friends;
        }

        public List<Friend> GetFriends(int friendId)
        {
            var friends = _context.Friends.Where(x => x.FriendContactID == friendId).ToList();
            return friends;
        }
    }
}