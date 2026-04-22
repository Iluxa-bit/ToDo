using ToDo_1.Models;
using ToDo_1.Logging;
namespace ToDo_1.Logging
{
    public class BdObserver: IObserverLog
    {
        private readonly IServiceScopeFactory scopeFactory;

        public BdObserver(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public void Update(Log log)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                dbContext.Logs.Add(log);
                dbContext.SaveChanges();
            }
        }
    }
}
