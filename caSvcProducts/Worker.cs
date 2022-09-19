using amLogger;
using caLibProdStat;

namespace caSvcProducts;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        InitializeLogger();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker start running at: {time}", DateTimeOffset.Now);

            bool done = false;
            ProductsUpdater.Start(() => done = true);
            while(!done)
                Thread.Sleep(1000);

            _logger.LogInformation("All done at: {time}", DateTimeOffset.Now);
            await Task.Delay(20 * 60 * 1000, stoppingToken);
        }
    }

    void InitializeLogger()
    {
        Logger.Instance.Init((Log log) =>
        {
            string msg = $"{log.src}: {log.msg}";
            _logger.Log((LogLevel)log.lvl, log.id, msg);
        });
    }
}