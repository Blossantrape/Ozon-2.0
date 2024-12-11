namespace Ozon.API.Services
{
    public class CalculatorService : ICalculatorService
    {
        public Task<double> AddAsync(double a, double b)
        {
            return Task.FromResult(a + b);
        }

        public Task<double> SubtractAsync(double a, double b)
        {
            return Task.FromResult(a - b);
        }

        public Task<double> MultiplyAsync(double a, double b)
        {
            return Task.FromResult(a * b);
        }

        public Task<double> DivideAsync(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Деление на ноль невозможно.");
            }

            return Task.FromResult(a / b);
        }
    }
}