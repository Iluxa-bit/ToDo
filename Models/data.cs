using Microsoft.EntityFrameworkCore;
using ToDo_1.ClassesDTO;
namespace ToDo_1.Models
{
 
    public class ApplicationContext : DbContext
    {
        public DbSet<Purpose> Tasks { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Purpose>().HasData(
                    new Purpose() { Id = 1, Title = "Задача1", Description = "Описание задачи1", Status = StatusDto.NotStarted },
                    new Purpose() { Id = 2, Title = "Задача2", Description = "Описание задачи2", Status = StatusDto.NotStarted },
                    new Purpose() { Id = 3, Title = "Задача3", Description = "Описание задачи3", Status = StatusDto.NotStarted }
            );
        }

    }
}
