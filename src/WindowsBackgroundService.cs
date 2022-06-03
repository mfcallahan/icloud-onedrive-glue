using Microsoft.Extensions.Options;

namespace iCloudOneDriveGlue
{
    public class WindowsBackgroundService : BackgroundService
    {
        private readonly ILogger<WindowsBackgroundService> _logger;
		private readonly AppSettings _appSettings;
		private FileSystemWatcher _watcher;

		public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger, IOptions<AppSettings> appSettings)
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
            _watcher = new FileSystemWatcher(_appSettings.ICloudPath)
            {
                NotifyFilter =     NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
            };

            _watcher.Created += OnCreated;

			_watcher.IncludeSubdirectories = true;
			_watcher.EnableRaisingEvents = true;
		}

		private void OnCreated(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine($"OnCreated: {e.FullPath}");

            try
            {
				File.Copy(e.FullPath, Path.Combine(_appSettings.OneDrivePath, e.Name));
			}
			catch
            {
				// just ignore it for now :/
            }
		}

		private void LogError()
		{
			throw new NotImplementedException();	
		}
	}
}
