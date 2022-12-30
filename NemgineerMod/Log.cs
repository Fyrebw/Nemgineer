using BepInEx.Logging;

namespace NemgineerMod
{
    internal static class Log
    {
        internal static ManualLogSource _logSource;

        internal static void Init(ManualLogSource logSource) => Log._logSource = logSource;

        internal static void Debug(object data) => Log._logSource.LogDebug(data);

        internal static void Error(object data) => Log._logSource.LogError(data);

        internal static void Fatal(object data) => Log._logSource.LogFatal(data);

        internal static void Info(object data) => Log._logSource.LogInfo(data);

        internal static void Message(object data) => Log._logSource.LogMessage(data);

        internal static void Warning(object data) => Log._logSource.LogWarning(data);
    }
}