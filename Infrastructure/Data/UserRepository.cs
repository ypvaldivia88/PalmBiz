using SQLite;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public UserRepository(SQLiteAsyncConnection db)
        {
            _db = db;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _db.Table<User>().ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _db.Table<User>().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _db.Table<User>().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            // Set sync fields
            user.LastModified = DateTime.UtcNow;
            user.IsSynced = false;
            
            await _db.InsertAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            // Set sync fields
            user.LastModified = DateTime.UtcNow;
            user.IsSynced = false;
            
            await _db.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                // Soft delete
                user.IsDeleted = true;
                user.IsActive = false;
                user.LastModified = DateTime.UtcNow;
                user.IsSynced = false;
                
                await _db.UpdateAsync(user);
            }
        }
    }
}