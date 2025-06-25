using PLCVirtual_NModbusMultipleSlave;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((hostContext, logging) =>
    {
        // ensure just use NLog
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        //services.AddWindowsService(options =>
        //{
        //    options.ServiceName = "your.windowsappname";
        //});
    })
    .Build();

host.Run();