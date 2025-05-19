using SQLite;
using Core.Entities;

namespace Infrastructure.Data
{
    public class SaleRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public SaleRepository(SQLiteAsyncConnection db)
        {
            _db = db;
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _db.Table<Sale>().ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _db.Table<Sale>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Sale sale)
        {
            await _db.InsertAsync(sale);
        }

        public async Task UpdateAsync(Sale sale)
        {
            await _db.UpdateAsync(sale);
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await GetByIdAsync(id);
            if (sale != null)
            {
                await _db.DeleteAsync(sale);
            }
        }
    }
}