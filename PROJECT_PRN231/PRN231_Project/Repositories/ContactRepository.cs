using Microsoft.EntityFrameworkCore;
using PRN231_Project.Interfaces;
using PRN231_Project.Models;

namespace PRN231_Project.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly Project_PRN231Context _context;
        public ContactRepository(Project_PRN231Context context)
        {
            _context = context;
        }
        public bool ContactExists(int id)
        {
            return _context.Contacts.Any(c => c.Id == id);
        }
        public Contact GetContactById(int id)
        {
            return _context.Contacts.FirstOrDefault(p => p.Id == id);
        }
        public bool CreateContact(Contact contact)
        {
            _context.Add(contact);
            return Save();
        }

        public bool DeleteContact(int id)
        {
            var contact = GetContactById(id);
            var con_la = _context.ContactLabels.Where(p => p.ContactId == contact.Id).ToList();
            if(con_la.Count > 0)
            {
                _context.ContactLabels.RemoveRange(con_la);
            }           
            _context.Contacts.Remove(contact);
            return Save();
        }

        public ICollection<Contact> GetContactByName(int id, string name)
        {
            return _context.Contacts.Where(p => p.UserId == id).Where(c => c.FullName.Trim().ToLower().Equals(name.Trim().ToLower())).ToList();
        }

        public ICollection<Contact> GetContactsByUserId(int id)
        {
            return _context.Contacts.Where(p => p.UserId == id).OrderBy(p => p.FullName).ToList();
        }

        public ICollection<Label> GetLabelsByContact(int id)
        {
            return _context.ContactLabels.Where(p => p.ContactId == id).Select(p => p.Label).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateContact(Contact contact)
        {
            _context.Contacts.Update(contact);
            return Save();
        }

        public ICollection<Contact> GetPopularContactsByUserId(int id)
        {
            return _context.Contacts.Where(p => p.UserId == id && p.VisitedCount>1).OrderByDescending(p => p.VisitedCount).ThenBy(p => p.FullName).Take(5).ToList();
        }
    }
}
