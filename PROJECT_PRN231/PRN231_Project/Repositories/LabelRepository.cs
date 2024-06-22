using PRN231_Project.Interfaces;
using PRN231_Project.Models;

namespace PRN231_Project.Repositories
{
    public class LabelRepository : ILabelRepository
    {
        private readonly Project_PRN231Context _context;
        public LabelRepository(Project_PRN231Context context)
        {
            _context = context;
        }
        public bool Create(Label label)
        {
            _context.Labels.Add(label);
            return Save();
        }

        public bool Delete(Label label)
        {
            var con_la = _context.ContactLabels.Where(p => p.LabelId == label.Id).ToList();
            if (con_la.Count > 0)
            {
                _context.ContactLabels.RemoveRange(con_la);
            }
            _context.Labels.Remove(label);
            return Save();
        }

        public Label GetLabel(int id)
        {
            var label= _context.Labels.FirstOrDefault(p => p.Id == id);
            if(label == null) 
            {
                return new Label();
            }
            return label;
        }

        public ICollection<Label> GetLabels()
        {
            return _context.Labels.ToList();
        }

        public bool Update(Label label)
        {
            _context.Labels.Update(label);
            return Save();
        }
        public ICollection<Contact> GetContactsByLabel(int id)
        {
            var contacts = _context.ContactLabels.Where(p => p.LabelId==id && p.Contact.IsInTrash==false).Select(p => p.Contact).ToList();
            return contacts;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
