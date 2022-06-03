using Microsoft.Extensions.Options;

namespace iCloudOneDriveGlue
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
		private readonly AppSettings _appSettings;
		private FileSystemWatcher _watcher;

		public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
			_appSettings = appSettings.Value;

			
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			InitWatcher();

			return base.StartAsync(cancellationToken);
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			await Task.CompletedTask;
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			await base.StopAsync(cancellationToken);
		}

		private void InitWatcher()
        {
			_watcher = new FileSystemWatcher(_appSettings.ICloudPath);

			_watcher.NotifyFilter = NotifyFilters.Attributes
								 | NotifyFilters.CreationTime
								 | NotifyFilters.DirectoryName
								 | NotifyFilters.FileName
								 | NotifyFilters.LastAccess
								 | NotifyFilters.LastWrite
								 | NotifyFilters.Security
								 | NotifyFilters.Size;

			_watcher.Created += OnCreated;

			_watcher.IncludeSubdirectories = true;
			_watcher.EnableRaisingEvents = true;
		}

		private static void OnCreated(object sender, FileSystemEventArgs e)
		{
			string value = $"Created: {e.FullPath}";
			Console.WriteLine(value);

			CopyFiles();
		}

		private static void CopyFiles()
        {

        }

		// TODO: log to file
		private void LogError()
		{
			throw new NotImplementedException();	
		}
	}
}