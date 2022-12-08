namespace Main.Slices.Rota
{
    public sealed class RepeatingService : BackgroundService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public RepeatingService(Serilog.ILogger logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromHours(1)); // every 24 hours

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken)
                && !stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IDailyRotaUpdate>();
                    await service.Init();
                    _logger.Information($"Rota function called running at: {DateTime.UtcNow}");
                }
            }
        }
    }
}