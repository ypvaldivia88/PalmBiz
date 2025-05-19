using Core.Entities;

namespace Core.Interfaces
{
    public interface ISaleService
    {
        IEnumerable<Sale> GetAll();
        Sale GetById(int id);
        void Add(Sale sale);
        void Update(Sale sale);
        void Delete(int id);
    }

}
