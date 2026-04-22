using ToDo_1.Models;
namespace ToDo_1.Logging
{

        public interface IObservableLog
        {
            public void AddObserver(IObserverLog o);
            public void RemoveObserver(IObserverLog o);
            public void NotifyObservers(Log communication);

        }
    
}
