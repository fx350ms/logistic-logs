using Abp;
using Abp.Castle.Logging.Log4Net;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Castle.Facilities.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Logistics.Logs.Migrator;

public class Program
{
    private static bool _quietMode = false;

    public static void Main(string[] args)
    {
        ParseArgs(args);

        using (var bootstrapper = AbpBootstrapper.Create<LogsMigratorModule>())
        {
            // Load & register IConfiguration BEFORE Initialize()
            var configuration = BuildConfiguration();

            bootstrapper.IocManager.IocContainer.Register(
                Castle.MicroKernel.Registration.Component
                    .For<IConfiguration>()
                    .Instance(configuration)
                    .LifestyleSingleton()
            );

            // Enable log4net
            bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );

            // Initialize ABP + Modules
            bootstrapper.Initialize();

            using (var migrateExecuter = bootstrapper.IocManager.ResolveAsDisposable<MultiTenantMigrateExecuter>())
            {
                var migrationSucceeded = migrateExecuter.Object.Run(_quietMode);

                if (_quietMode)
                {
                    var exitCode = Convert.ToInt32(!migrationSucceeded);
                    Environment.Exit(exitCode);
                }
                else
                {
                    Console.WriteLine("Press ENTER to exit...");
                    Console.ReadLine();
                }
            }
        }
    }

    // -----------------------------
    // Build Configuration from appsettings.json
    // -----------------------------
    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())       // Path của Migrator.exe
            .AddJsonFile("appsettings.json", optional: false)   // Load connection string
            .AddEnvironmentVariables()                         // Enable env override
            .Build();
    }

    // -----------------------------
    // Parse command-line arguments
    // -----------------------------
    private static void ParseArgs(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            return;
        }

        foreach (var arg in args)
        {
            switch (arg)
            {
                case "-q":
                    _quietMode = true;
                    break;
            }
        }
    }
}