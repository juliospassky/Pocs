
using Application.Adapter.InBound;
using Microsoft.Extensions.DependencyInjection;

Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<TodoInBound>();
    }).Build().StartAsync();