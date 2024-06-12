using PRN231_Project.Interfaces;
using PRN231_Project.Models;

namespace PRN231_Project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Project_PRN231Context _context;
        public UserRepository(Project_PRN231Context context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return Save();

        }


        public User? GetByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username.Equals(username));
            if(user == null)
            {
                return null;
            }
            return user;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();

        }

        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);    
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
