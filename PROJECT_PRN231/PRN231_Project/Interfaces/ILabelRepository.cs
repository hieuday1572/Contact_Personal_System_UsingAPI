using PRN231_Project.Models;

namespace PRN231_Project.Interfaces
{
    public interface ILabelRepository
    {
        public ICollection<Models.Label> GetLabels();
        public Models.Label GetLabel(int id);
        public bool Create(Models.Label label);
        public bool Update(Models.Label label);
        public bool Delete(Models.Label label);
        public ICollection<Contact> GetContactsByLabel(int id);

    }
}
