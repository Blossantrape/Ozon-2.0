using Ozon.Application.Abstractions;

namespace Ozon.API.BackgroundServices
{
    public class StockCheckingBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<StockCheckingBackgroundService> _logger;

        public StockCheckingBackgroundService(IServiceScopeFactory scopeFactory,
            ILogger<StockCheckingBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stock Checking Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking stock at: {time}", DateTimeOffset.Now);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var stockService = scope.ServiceProvider.GetRequiredService<IStockService>();
                    await stockService.CheckStockAsync();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Запускать проверку каждый час
            }

            _logger.LogInformation("Stock Checking Background Service is stopping.");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stock Checking Background Service is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}