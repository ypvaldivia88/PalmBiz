using Core.Entities;
using Core.Interfaces;


namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new();

        public IEnumerable<Product> GetAll() => _products;

        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product) => _products.Add(product);

        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1) _products[index] = product;
        }

        public void Delete(int id) => _products.RemoveAll(p => p.Id == id);
    }

}
