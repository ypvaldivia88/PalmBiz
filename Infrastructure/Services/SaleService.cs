using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SaleService : ISaleService
    {
        private readonly SaleRepository _repository;

        public SaleService(SaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Sale sale)
        {
            await _repository.AddAsync(sale);
        }

        public async Task UpdateAsync(Sale sale)
        {
            await _repository.UpdateAsync(sale);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
