using PRN231_Project.Models;

namespace PRN231_Project.Interfaces
{
    public interface IContactRepository
    {
        ICollection<Contact> GetContactsByUserId(int id);
        Contact GetContactById(int id);
        ICollection<Contact> GetContactByName(int userId, string name);
        ICollection<Label> GetLabelsByContact(int id);
        bool ContactExists(int id);
        bool CreateContact(Contact contact);
        bool UpdateContact(Contact contact);
        bool DeleteContact(int id);
        bool Save();
    }
}
