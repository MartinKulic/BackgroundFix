using BackBroundFix_WorkerService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics.Metrics;


//Huge thanks to Louie - https://stackoverflow.com/questions/20261523/detect-number-of-display-monitor

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "AAA_Test";
});

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

builder.Services.AddSingleton<WatcherService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
