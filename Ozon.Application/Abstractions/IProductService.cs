using Ozon.Core.Models;

namespace Ozon.Application.Abstractions
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product GetById(Guid id);
        void Add(Product product);
        void Update(Product product);
        void Delete(Guid id);

        public void AttachProduct(Guid id);
    }
}