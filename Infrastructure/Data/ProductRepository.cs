using SQLite;
using Core.Entities;

namespace Infrastructure.Data
{
    public class ProductRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public ProductRepository(SQLiteAsyncConnection db)
        {
            _db = db;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _db.Table<Product>().ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Table<Product>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _db.InsertAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _db.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                await _db.DeleteAsync(product);
            }
        }
    }
}
