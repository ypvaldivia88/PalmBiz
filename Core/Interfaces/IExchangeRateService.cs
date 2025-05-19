using Core.Entities;

namespace Core.Interfaces
{
    public interface IExchangeRateService
    {
        IEnumerable<ExchangeRate> GetAll();
        ExchangeRate GetById(int id);
        void Add(ExchangeRate exchangeRate);
        void Update(ExchangeRate exchangeRate);
        void Delete(int id);
    }

}
