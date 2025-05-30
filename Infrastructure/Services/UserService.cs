using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _repository.GetByUsernameAsync(username);
        }

        public async Task AddAsync(User user)
        {
            await _repository.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _repository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
