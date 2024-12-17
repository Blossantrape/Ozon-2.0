using Ozon.Application.Abstractions;
using Ozon.DataAccess.Abstractions;

namespace Ozon.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task CheckStockAsync()
        {
            var warehouses = await _stockRepository.GetAllWarehousesWithProductsAsync();

            foreach (var warehouse in warehouses)
            {
                foreach (var product in warehouse.Products)
                {
                    if (product.StockQuantity < 10) // Пример проверки минимального количества
                    {
                        // Логика обработки недостаточного количества товара
                        Console.WriteLine(
                            $"Склад {warehouse.Name}: недостаточное количество товара {product.Name} (ID: {product.Id})");
                    }
                }
            }
        }
    }
}