using Logistics.Logs.Debugging;

namespace Logistics.Logs;

public class LogsConsts
{
    public const string LocalizationSourceName = "Logs";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "d7b6dda83e154eb1965d3ff2f5881be4";
}
