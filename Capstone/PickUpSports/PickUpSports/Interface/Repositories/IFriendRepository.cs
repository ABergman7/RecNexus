using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IFriendRepository
    {
        Friend AddFriend(Friend friend);

        void Delete(Friend friend);

        List<Friend> GetContactsFriends(int contactId);

        List<Friend> GetFriends(int friendId);
    }
}