using ToDo_1.Models;
namespace ToDo_1.Logging
{
    public class FileObserver : IObserverLog
    {
        private readonly string filePath;
        private static object _lock = new();

        public FileObserver(string filePath)
        {
            this.filePath = filePath;
        }

        public void Update(Log log)
        {
            lock (_lock)
            {
                File.AppendAllText(filePath, (log.Message, log.DateTime) + Environment.NewLine);
            }
        }
    }
}
