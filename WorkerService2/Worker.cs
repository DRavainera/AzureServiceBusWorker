namespace WorkerService2
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMiAzureServiceBus _serviceBus;
        private readonly PeriodicTimer _timer =
            new(TimeSpan.FromMilliseconds(Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["timer"])));

        public Worker(ILogger<Worker> logger, IMiAzureServiceBus serviceBus, PeriodicTimer timer)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _timer = timer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker corriendo en: {time}", DateTimeOffset.Now);

                await _serviceBus.GetQueues(stoppingToken);
            }
        }
    }
}