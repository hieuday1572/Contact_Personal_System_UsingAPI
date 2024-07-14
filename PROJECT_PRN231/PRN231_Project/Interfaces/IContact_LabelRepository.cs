using PRN231_Project.Models;

namespace PRN231_Project.Interfaces
{
    public interface IContact_LabelRepository
    {
        public bool Create(ContactLabel contactLabel);
        public bool Delete(int contactId, int labelId);
        public ContactLabel GetByContactIdAndLabelId(int contactId, int labelId);
    }
}
