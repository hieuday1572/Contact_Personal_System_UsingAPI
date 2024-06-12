using PRN231_Project.Models;

namespace PRN231_Project.Interfaces
{
    public interface IUserRepository
    {
        public User? GetByUsername(string username);
        public bool CreateUser(User user);
        public bool Save();
        public bool UpdateUser(User user);
        public ICollection<User> GetUsers();
    }
}
