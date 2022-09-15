using caSvcProducts;
using NLog.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseWindowsService()
    .UseNLog()
    .Build();

await host.RunAsync();
