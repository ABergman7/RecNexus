using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IContactRepository
    {
        Contact GetContactByEmail(string email);

        Contact GetContactById(int? id);

        Contact GetContactByUsername(string username);

        Contact CreateContact(Contact contact);

        void EditContact(Contact contact);

        void DeleteContact(Contact contact);
    }
}
