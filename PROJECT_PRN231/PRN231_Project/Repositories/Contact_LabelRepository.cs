using Microsoft.EntityFrameworkCore;
using PRN231_Project.Interfaces;
using PRN231_Project.Models;

namespace PRN231_Project.Repositories
{
    public class Contact_LabelRepository : IContact_LabelRepository
    {
        private readonly Project_PRN231Context _context;
        public Contact_LabelRepository(Project_PRN231Context context)
        {
            _context = context;
        }
        public bool Create(ContactLabel contactLabel)
        {
            _context.ContactLabels.Add(contactLabel);
            return Save();
        }

        public bool Delete(ContactLabel contactLabel)
        {
            _context.ContactLabels.Remove(contactLabel);
            return Save();
        }

        public ContactLabel GetByContactIdAndLabelId(int contactId, int labelId)
        {
            ContactLabel? item = _context.ContactLabels.FirstOrDefault(p => p.ContactId == contactId && p.LabelId == labelId);
            if(item == null)
            {
                return new ContactLabel();
            }
            return item;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
