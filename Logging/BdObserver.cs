using ToDo_1.Models;
using ToDo_1.Logging;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
namespace ToDo_1.Logging
{
    public class BdObserver: FormatLogMesssage, IObserverLog
    {
        private readonly IServiceScopeFactory scopeFactory;

        protected override string EditorMessage(Log log)
        {
            return $"Обновлённое сообщение {log.Message}";
        }
        public BdObserver(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public void Update(Log log)
        {
            log.Message=EditorMessage(log);
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                dbContext.Logs.Add(log);
                dbContext.SaveChanges();
            }
        }
    }
}
