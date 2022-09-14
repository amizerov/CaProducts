using caSvcProducts;
using NLog.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseNLog()
    .Build();

await host.RunAsync();
