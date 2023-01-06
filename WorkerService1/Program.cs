using Azure.Messaging.ServiceBus;
using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["AzureConexion"].ConnectionString;

        services.AddSingleton((s) =>
        {
            return new ServiceBusClient(conexion, new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets });
        });

        services.AddSingleton<IMiAzureServiceBus, MiAzureServiceBus>();

        services.AddDbContext<ApplicationDbContext>();

    })
    .UseWindowsService()
    .Build();

await host.RunAsync();
