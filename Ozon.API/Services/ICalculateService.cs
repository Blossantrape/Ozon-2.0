namespace Ozon.API.Services;

public interface ICalculatorService
{
    Task<double> AddAsync(double a, double b);
    Task<double> SubtractAsync(double a, double b);
    Task<double> MultiplyAsync(double a, double b);
    Task<double> DivideAsync(double a, double b);
}