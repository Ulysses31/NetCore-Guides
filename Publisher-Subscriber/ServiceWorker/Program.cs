using ServiceWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<HttpClient>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
