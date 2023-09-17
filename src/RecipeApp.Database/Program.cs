
namespace RecipeApp.Database;

//SqlScriptOptions SqlScriptOptions;
//SqlScriptOptions.ScriptType = DbUp.Support.ScriptType.RunOnce;
//SqlScriptOptions.ScriptType = DbUp.Support.ScriptType.RunAlways;

//FileSystemScriptOptions FileSystemScriptOptions = new();
//FileSystemScriptOptions.Extensions = new[] { ".sql" };
//FileSystemScriptOptions.IncludeSubDirectories = true;

internal static class Program
{
    static IHostEnvironment HostEnvironment { get; set; } = null!;

    static IConfiguration Configuration { get; set; } = null!;

    static string DefaultConnectionString { get; set; } = null!;

    /// <summary>
    /// Main entry point.
    /// Don't forget to set command line arguments:
    /// </summary>
    /// <param name="args"></param>
    static int Main(string[] args)
    {
        int exitCode = 0;
        DatabaseUpgradeResult runOnceResult = new(Enumerable.Empty<SqlScript>(), false, null, null);
        DatabaseUpgradeResult runAlwaysResult = new(Enumerable.Empty<SqlScript>(), false, null, null);
        try
        {
            CreateSerilogLogger();
            Log.Information("{V}", "Process Starting");

            Log.Information("Creating Host");
            using IHost host = CreateHostBuilder(args);

            runOnceResult = InvokeRunOnceScripts(DefaultConnectionString);

            if (runOnceResult.Successful)
                runAlwaysResult = InvokeRunAlwaysScripts(DefaultConnectionString);
            else
                Log.Information("RunOnce Scripts was NOT Successful... Skipping RunAlways Scripts");

            exitCode = 1;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception occurred: ");
        }
        finally
        {
            Log.Information("{V}", $"Process Exiting: ExitCode={exitCode}; RunOnce Result={runOnceResult.Successful}; RunAlways Result={runAlwaysResult.Successful}{Environment.NewLine}");
            Log.CloseAndFlush();
        }

#if DEBUG
        Console.ReadLine();
#endif
        return exitCode;
    }

    static IHost CreateHostBuilder(string[] args)
    {
        // https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage
        return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    configuration.AddCommandLine(args);

                    configuration
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile("appsettings.staging.json", false, true)
                        .AddJsonFile("appsettings.development.json", false, true);

                    IConfigurationRoot configurationRoot = configuration.Build();
                    Configuration = configurationRoot;

                    var commandLineArgEnv = configurationRoot.GetValue<string>("Env");
                    var possibleEnvironmentNames = new List<string> { "development", "staging", "production" };
                    if (string.IsNullOrWhiteSpace(commandLineArgEnv) || !possibleEnvironmentNames.Contains(commandLineArgEnv.ToLower()))
                        commandLineArgEnv = "development";

                    HostEnvironment = hostingContext.HostingEnvironment;
                    HostEnvironment.EnvironmentName = commandLineArgEnv;
                })
                .ConfigureServices((_, services) =>
                {
                    var defaultConnectionString = Configuration.GetConnectionString("Default");
                    DefaultConnectionString = defaultConnectionString;
                })
                .Build();
    }

    static void CreateSerilogLogger()
    {
        // optional has to be false for serilog file sink to work
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.staging.json", false, true)
            .AddJsonFile("appsettings.development.json", false, true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    static DatabaseUpgradeResult InvokeRunOnceScripts(string connectionString)
    {
        Log.Information("{V}", "Invoking RunOnce Scripts...");
        var upgrader = DeployChanges.To
                       .SqlDatabase(connectionString)
                       .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), f => f.Contains(".RunOnce."))
                       .WithTransactionPerScript()
                       .WithExecutionTimeout(TimeSpan.FromSeconds(180))
                       .LogToAutodetectedLog()
                       .LogScriptOutput()
                       .Build();

        return upgrader.PerformUpgrade();
    }

    static DatabaseUpgradeResult InvokeRunAlwaysScripts(string connectionString)
    {
        Log.Information("{V}", "Invoking RunAlways Scripts...");
        var upgrader = DeployChanges.To
                       .SqlDatabase(connectionString)
                       .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), f => f.Contains(".RunAlways."))
                       .WithTransactionPerScript()
                       .WithExecutionTimeout(TimeSpan.FromSeconds(180))
                       .JournalTo(new NullJournal())
                       .LogToAutodetectedLog()
                       .LogScriptOutput()
                       .Build();

        return upgrader.PerformUpgrade();
    }
}