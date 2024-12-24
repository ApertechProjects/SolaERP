using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace SolaERP.API.Methods
{
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<string, (DateTime ResetTime, int RequestCount)> Clients = new();
        private readonly int _requestLimit;
        private readonly TimeSpan _timeWindow;

        public RateLimitAttribute(int requestLimit, int seconds)
        {
            _requestLimit = requestLimit;
            _timeWindow = TimeSpan.FromSeconds(seconds);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var clientIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (clientIp == null)
            {
                base.OnActionExecuting(context);
                return;
            }

            var now = DateTime.UtcNow;
            var clientRecord = Clients.GetOrAdd(clientIp, _ => (ResetTime: now.Add(_timeWindow), RequestCount: 0));

            // Check if time window has expired
            if (now > clientRecord.ResetTime)
            {
                Clients[clientIp] = (ResetTime: now.Add(_timeWindow), RequestCount: 1);
            }
            else
            {
                if (clientRecord.RequestCount >= _requestLimit)
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = StatusCodes.Status429TooManyRequests,
                        Content = $"Too many requests. Please wait until {clientRecord.ResetTime.ToString("T")}."
                    };
                    return;
                }

                Clients[clientIp] = (clientRecord.ResetTime, clientRecord.RequestCount + 1);
            }

            base.OnActionExecuting(context);
        }
    }
}
