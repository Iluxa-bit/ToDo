using ToDo_1.Models;
namespace ToDo_1.Logging
{
    public class FileObserver : FormatLogMesssage, IObserverLog
    {
        private readonly string filePath;
        private static object _lock = new();

        public FileObserver(string filePath)
        {
            this.filePath = filePath;
        }

        protected override string EditorMessage(Log log)
        {
            return $"Обновлённое сообщение {log.Message}{log.DateTime}";
        }

        public void Update(Log log)
        {
            lock (_lock)
            {
                File.AppendAllText(filePath, EditorMessage(log) + Environment.NewLine);
            }
        }
    }
}
