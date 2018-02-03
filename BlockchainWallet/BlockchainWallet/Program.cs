namespace BlockchainWallet
{
    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using Serilog.Debugging;
    using Serilog.Sinks.SystemConsole.Themes;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .MinimumLevel.Verbose()
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment)
                        .Enrich.WithProperty("HostName", Environment.MachineName)
                        .WriteTo.Console(theme: SystemConsoleTheme.Literate);

                    SelfLog.Enable(Console.Error);
                })
                .Build();
    }
}