using AngleSharp.Io;
using Confluent.Kafka;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Dtos.Shared;
using System.Text.Json;

namespace SolaERP.API.Middlewares
{
    public class RequestLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private static Dictionary<string, (DateTime resetTime, int requestCount)> _clients = new();
        private readonly int _limit = 3; // Maximum requests allowed
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1); // Time window
        private readonly List<string> _rateLimitedPaths;


        public RequestLimitMiddleware(RequestDelegate next, List<string> rateLimitedPaths)
        {
            _next = next;
            _rateLimitedPaths = rateLimitedPaths;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestPath = context.Request.Path.Value?.Split('?')[0];
            if (_rateLimitedPaths.Contains(requestPath))
            {
                var clientIp = context.Connection.RemoteIpAddress?.ToString();
                if (clientIp == null)
                {
                    await _next(context);
                    return;
                }

                if (!_clients.ContainsKey(clientIp))
                {
                    _clients[clientIp] = (DateTime.UtcNow.Add(_timeWindow), 0);
                }

                var (resetTime, requestCount) = _clients[clientIp];

                if (DateTime.UtcNow > resetTime)
                {
                    _clients[clientIp] = (DateTime.UtcNow.Add(_timeWindow), 1);
                }
                else if (requestCount >= _limit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Too many requests. Try again later.");
                    return;
                }
                else
                {
                    _clients[clientIp] = (resetTime, requestCount + 1);
                }
            }

            await _next(context);
        }
    }

}

