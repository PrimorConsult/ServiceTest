using System;
using System.IO;
using System.ServiceProcess;

namespace MappingService
{
    public partial class Service1 : ServiceBase
    {
        private FileSystemWatcher _watcher;
        private const string _folderToWatch = @"C:\Users\sps02\Downloads\PastaTeste";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists("MappingService"))
                {
                    System.Diagnostics.EventLog.CreateEventSource("MappingService", "Application");
                }

                _watcher = new FileSystemWatcher
                {
                    Path = _folderToWatch,
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                    Filter = "*.*"
                };

                _watcher.Created += OnCreated;
                _watcher.Deleted += OnDeleted;
                _watcher.Renamed += OnRenamed;
                _watcher.EnableRaisingEvents = true;

                MyLogEvent("Serviço iniciado e monitorando a pasta...");
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("MappingService", $"Error in OnStart : {ex.Message}", System.Diagnostics.EventLogEntryType.Error);
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
           MyLogEvent($"O Arquivo foi renomeado de {e.OldFullPath} to {e.FullPath}");
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            MyLogEvent($"O Arquivo foi deletado: {e.FullPath}");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            MyLogEvent($"O Arquivo foi criado: {e.FullPath}");
        }

        protected override void OnStop()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
        }

        private void MyLogEvent(string message)
        {
            var newFolder = Path.Combine(@"C:\Users\sps02\Downloads\PastaTeste", "Logs");
            if (!Directory.Exists(newFolder)) {Directory.CreateDirectory(newFolder);}

            var filePath = Path.Combine(newFolder, "LogService.txt");
            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - {Environment.MachineName} - : {message}");
            }

            // Também escreve no Event Viewer
            System.Diagnostics.EventLog.WriteEntry("MappingService", message, System.Diagnostics.EventLogEntryType.Information);
        }
    }
}
