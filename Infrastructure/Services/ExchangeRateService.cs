using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ExchangeRateRepository _repository;

        public ExchangeRateService(ExchangeRateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ExchangeRate>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ExchangeRate> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(ExchangeRate exchangeRate)
        {
            await _repository.AddAsync(exchangeRate);
        }

        public async Task UpdateAsync(ExchangeRate exchangeRate)
        {
            await _repository.UpdateAsync(exchangeRate);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
