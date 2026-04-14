using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ToDo_1.Logging
{

    public interface IObservable
    {
        public void AddObserver(IObserver observer);
        public void RemoveObserver(IObserver observer);
        public void NotifyObservers(LoggData loggData);
    }

    public interface IObserver
    {
       public void Update(LoggData loggData);
    }

    public class LoggData
    {
        public string message { get; set; } = "";

        public LogLevel level { get; set; }

        public DateTime dateTime { get; set; }
        public Exception? Exception { get; set; }
    }
    public class ObservableLogger : IObservable,ILogger, IDisposable
    {
        
        public ObservableLogger()
        {
            
        }
        List<IObserver> observers = new List<IObserver>();
        public void AddObserver(IObserver observer) 
        {
            
            observers.Add(observer);
        }
        public void RemoveObserver(IObserver observer) 
        {
            observers.Remove(observer);
        
        }

        public void NotifyObservers(LoggData loggData) 
        { 
          foreach(IObserver observer in observers.ToList())
            {
                observer.Update(loggData);
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose() { }

        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId,
                    TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {

            LoggData loggData = new LoggData{ message = formatter(state, exception),level = logLevel };

            NotifyObservers(loggData);
            
        }

    }

    public class FileObserver : IObserver
    {
        private readonly string filePath;
        public FileObserver(string filePath)
        {
            this.filePath = filePath;
        }
        static object _lock = new object();
        public void Update(LoggData loggData)
        {
            string line = $"{loggData.level}: {loggData.message}";
            lock (_lock)
            {
                File.AppendAllText(filePath, line + Environment.NewLine);
            }
        }
        
    }
}
