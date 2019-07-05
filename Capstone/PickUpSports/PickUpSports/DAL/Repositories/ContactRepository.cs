using System;
using System.Data.Entity;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly PickUpContext _context;

        public ContactRepository(PickUpContext context)
        {
            _context = context;
        }

        public Contact GetContactByEmail(string email)
        {
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email == email);
            if (contact == null) return null;
            return contact;
        }

        public Contact GetContactById(int? id)
        {
            Contact contact = _context.Contacts.Find(id);
            if (contact == null) return null;
            return contact;
        }

        public Contact GetContactByUsername(string username)
        {
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Username == username);
            if (contact == null) return null;
            return contact;
        }

        public Contact CreateContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();

            var created = GetContactByEmail(contact.Email);
            return created;
        }

        public void EditContact(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteContact(Contact contact)
        {
            Contact existing = _context.Contacts.Find(contact.ContactId);
            if (existing == null) throw new ArgumentNullException($"Could not find existing contact by ID {contact.ContactId}");

            _context.Contacts.Remove(existing);
            _context.SaveChanges();
        }
    }
}