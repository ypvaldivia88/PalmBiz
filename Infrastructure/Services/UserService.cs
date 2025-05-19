using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();

        public IEnumerable<User> GetAll() => _users;

        public User GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public void Add(User user) => _users.Add(user);

        public void Update(User user)
        {
            var index = _users.FindIndex(u => u.Id == user.Id);
            if (index != -1) _users[index] = user;
        }

        public void Delete(int id) => _users.RemoveAll(u => u.Id == id);
    }

}
