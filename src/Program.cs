using iCloudOneDriveGlue;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

