namespace Ozon.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}