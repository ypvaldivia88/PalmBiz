using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class SaleService : ISaleService
    {
        private readonly List<Sale> _sales = new();

        public IEnumerable<Sale> GetAll() => _sales;

        public Sale GetById(int id) => _sales.FirstOrDefault(s => s.Id == id);

        public void Add(Sale sale) => _sales.Add(sale);

        public void Update(Sale sale)
        {
            var index = _sales.FindIndex(s => s.Id == sale.Id);
            if (index != -1) _sales[index] = sale;
        }

        public void Delete(int id) => _sales.RemoveAll(s => s.Id == id);
    }

}
