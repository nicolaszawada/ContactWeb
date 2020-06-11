using ContactWeb.Domain;
using System.Collections.Generic;
using System.Linq;

namespace ContactWeb.Database
{
    public interface IContactDatabase
    {
        Contact Insert(Contact movie);
        IEnumerable<Contact> GetContacts();
        Contact GetContact(int id);
        void Delete(int id);
        void Update(int id, Contact movie);
    }

    public class ContactDatabase : IContactDatabase
    {
        private int _counter;
        private readonly List<Contact> _contacts;

        public ContactDatabase()
        {
            if (_contacts == null)
            {
                _contacts = new List<Contact>();
            }
        }

        public Contact GetContact(int id)
        {
            return _contacts.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Contact> GetContacts()
        {
            return _contacts;
        }

        public Contact Insert(Contact movie)
        {
            _counter++;
            movie.Id = _counter;
            _contacts.Add(movie);
            return movie;
        }

        public void Delete(int id)
        {
            var movie = _contacts.SingleOrDefault(x => x.Id == id);
            if (movie != null)
            {
                _contacts.Remove(movie);
            }
        }

        public void Update(int id, Contact updatedContact)
        {
            var contact = _contacts.SingleOrDefault(x => x.Id == id);
            if (contact != null)
            {
                contact.FirstName = updatedContact.FirstName;
                contact.LastName = updatedContact.LastName;
                contact.Birthdate = updatedContact.Birthdate;
                contact.Address = updatedContact.Address;
                contact.Category = updatedContact.Category;
                contact.Email = updatedContact.Email;
                contact.PhoneNumber = updatedContact.PhoneNumber;
                contact.PhotoUrl = updatedContact.PhotoUrl;
            }
        }
    }
}
