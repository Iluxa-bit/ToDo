using ToDo_1.Models;

namespace ToDo_1.Logging
{
    public interface IObserverLog
    {
        public void Update(Log communication);
    }
}
