using System;
using System.IO;

namespace MappingService.Utils
{
    public static class Logger
    {
        private static readonly string logPath = @"C:\Logs\MappingService";

        public static void Log(string message)
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            var filePath = Path.Combine(logPath, "LogService.txt");
            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {Environment.MachineName} - : {message}");
            }

            System.Diagnostics.EventLog.WriteEntry("MappingService", message,
                System.Diagnostics.EventLogEntryType.Information);
        }
    }
}