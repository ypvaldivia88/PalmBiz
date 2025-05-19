using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRate>> GetAllAsync();
        Task<ExchangeRate> GetByIdAsync(int id);
        Task AddAsync(ExchangeRate exchangeRate);
        Task UpdateAsync(ExchangeRate exchangeRate);
        Task DeleteAsync(int id);
    }
}
