using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }

}
