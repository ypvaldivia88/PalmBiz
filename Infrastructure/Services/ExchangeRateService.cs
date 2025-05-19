using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly List<ExchangeRate> _rates = new();

        public IEnumerable<ExchangeRate> GetAll() => _rates;

        public ExchangeRate GetById(int id) => _rates.FirstOrDefault(r => r.Id == id);

        public void Add(ExchangeRate rate) => _rates.Add(rate);

        public void Update(ExchangeRate rate)
        {
            var index = _rates.FindIndex(r => r.Id == rate.Id);
            if (index != -1) _rates[index] = rate;
        }

        public void Delete(int id) => _rates.RemoveAll(r => r.Id == id);
    }

}
