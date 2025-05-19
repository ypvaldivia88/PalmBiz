using SQLite;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ExchangeRateRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public ExchangeRateRepository(SQLiteAsyncConnection db)
        {
            _db = db;
        }

        public async Task<List<ExchangeRate>> GetAllAsync()
        {
            return await _db.Table<ExchangeRate>().ToListAsync();
        }

        public async Task<ExchangeRate> GetByIdAsync(int id)
        {
            return await _db.Table<ExchangeRate>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(ExchangeRate rate)
        {
            await _db.InsertAsync(rate);
        }

        public async Task UpdateAsync(ExchangeRate rate)
        {
            await _db.UpdateAsync(rate);
        }

        public async Task DeleteAsync(int id)
        {
            var rate = await GetByIdAsync(id);
            if (rate != null)
            {
                await _db.DeleteAsync(rate);
            }
        }
    }
}