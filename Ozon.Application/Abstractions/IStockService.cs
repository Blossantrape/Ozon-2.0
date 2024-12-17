namespace Ozon.Application.Abstractions;

public interface IStockService
{
    Task CheckStockAsync(/*Guid productId, int quantity*/);
}