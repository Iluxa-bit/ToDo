using System.Diagnostics;
using ToDo_1.Models;
using ToDo_1.Logging;

namespace ToDo_1.Controllers
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate next;

        public RequestTimingMiddleware(RequestDelegate next)
        {
            this.next = next;

        }

        public async Task InvokeAsync(HttpContext context, IObservableLog observableLog)
        {

            Stopwatch stopwatch = Stopwatch.StartNew();
            var method = context.Request.Method;
            var path = context.Request.Path;

            await next.Invoke(context);


            stopwatch.Stop();
            var duration = stopwatch.ElapsedMilliseconds;
            var statuscode = context.Response.StatusCode;
            var log = new Log();
            log.Message = $"[TIMING] {method} {path} status code {statuscode} {duration} ms";
            log.DateTime = DateTime.UtcNow;
            observableLog.NotifyObservers(log);

        }
    }
}
