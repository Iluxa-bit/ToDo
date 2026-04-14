namespace ToDo_1.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        string path;
        public FileLoggerProvider(string path)
        {
            this.path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            var logger = new ObservableLogger();
            var observer = new FileObserver(path);
            logger.AddObserver(observer);
            return logger;
        }

        public void Dispose() { }
    }
}
