using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.IO;
using ToDo_1.ClassesDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ToDo_1.Logging;
using ToDo_1.Models;
namespace ToDo_1.Models
{

    public class ApplicationContext : DbContext, IObservableLog
    {
        
        List<IObserverLog> observers = new List<IObserverLog>();

        public void AddObserver(IObserverLog observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserverLog observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(Log communication)
        {
            foreach (IObserverLog observer in observers)
            {
                observer.Update(communication);
            }
        }


        public DbSet<Purpose> Tasks { get; set; } = null!;
        public DbSet<Log> Logs { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options,IServiceScopeFactory scopeFactory)
            : base(options)
        {
            AddObserver(new FileObserver("log.txt"));
            AddObserver(new BdObserver(scopeFactory));

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Purpose>().HasData(
                    new Purpose() { Id = 1, Title = "Задача1", Description = "Описание задачи1", Status = StatusRecord.NotStarted },
                    new Purpose() { Id = 2, Title = "Задача2", Description = "Описание задачи2", Status = StatusRecord.NotStarted },
                    new Purpose() { Id = 3, Title = "Задача3", Description = "Описание задачи3", Status = StatusRecord.NotStarted }
            );
        }

    }

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
