using Microsoft.EntityFrameworkCore;
using ToDo_1.ClassesDTO;
using System.IO;
using Microsoft.VisualBasic;
namespace ToDo_1.Models
{

    public class Communication
    {

        public int Id { get; set; }
        public string message { get; set; }
        public DateTime? dateTime { get; set; }
    }
    public interface IObserver
    {
        public void Update(Communication communication);
    }

    public interface IObservable
    {
        public void AddObserver(IObserver o);
        public void RemoveObserver(IObserver o);
        public void NotifyObservers ( Communication communication);

    }


    public class ApplicationContext : DbContext, IObservable
    {
        
        List<IObserver> observers = new List<IObserver>();

        public void AddObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(Communication communication)
        {
            foreach (IObserver observer in observers)
            {
                observer.Update(communication);
            }
        }


        public DbSet<Purpose> Tasks { get; set; } = null!;
        public DbSet<Communication> Communications { get; set; } = null!;
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
                    new Purpose() { Id = 1, Title = "Задача1", Description = "Описание задачи1", Status = StatusDto.NotStarted },
                    new Purpose() { Id = 2, Title = "Задача2", Description = "Описание задачи2", Status = StatusDto.NotStarted },
                    new Purpose() { Id = 3, Title = "Задача3", Description = "Описание задачи3", Status = StatusDto.NotStarted }
            );
        }

    }

    public class FileObserver : IObserver
    {
        private readonly string filePath;
        private static object _lock = new();

        public FileObserver(string filePath)
        {
            this.filePath = filePath;
        }

        public void Update(Communication communication)
        {
            lock (_lock)
            {
                File.AppendAllText(filePath, (communication.message, communication.dateTime) + Environment.NewLine);
            }
        }
    }

    public class BdObserver: IObserver
    {
        private readonly IServiceScopeFactory scopeFactory;

        public BdObserver(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public void Update(Communication communication)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                dbContext.Communications.Add(communication);
                dbContext.SaveChanges();
            }
        }
    }
}
