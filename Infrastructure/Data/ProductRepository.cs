using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
using SQLite;

namespace Infrastructure.Data
{
    public class ProductRepository
    {
        private readonly SQLiteAsyncConnection _db;
        private readonly ISyncService _syncService;

        public ProductRepository(SQLiteAsyncConnection db, ISyncService syncService)
        {
            _db = db;
            _syncService = syncService;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _db.Table<Product>()
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Table<Product>()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task AddAsync(Product product)
        {
            product.LastModified = DateTime.UtcNow;
            product.IsSynced = false;
            await _db.InsertAsync(product);
            
            // Try to sync immediately if possible
            try
            {
                await _syncService.SyncEntityAsync(product);
            }
            catch (Exception ex)
            {
                // Log the error but continue as the sync can happen later
                System.Diagnostics.Debug.WriteLine($"Sync error during add: {ex.Message}");
            }
        }

        public async Task UpdateAsync(Product product)
        {
            product.LastModified = DateTime.UtcNow;
            product.IsSynced = false;
            await _db.UpdateAsync(product);
            
            // Try to sync immediately if possible
            try
            {
                await _syncService.SyncEntityAsync(product);
            }
            catch (Exception ex)
            {
                // Log the error but continue as the sync can happen later
                System.Diagnostics.Debug.WriteLine($"Sync error during update: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                product.LastModified = DateTime.UtcNow;
                product.IsSynced = false;
                await _db.UpdateAsync(product);
                
                // Try to sync immediately if possible
                try
                {
                    await _syncService.SyncEntityAsync(product);
                }
                catch (Exception ex)
                {
                    // Log the error but continue as the sync can happen later
                    System.Diagnostics.Debug.WriteLine($"Sync error during delete: {ex.Message}");
                }
            }
        }
    }
}
