using AngleSharp.Io;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Dtos.Shared;
using System.Text.Json;

namespace SolaERP.API.Middlewares
{
    public class RequestLimitMiddleware
    {
        private static Dictionary<string, (DateTime, int)> _requestTracker = new();
        private readonly RequestDelegate _next;
        private readonly int _limit = 3; // Sorğu limiti
        private readonly TimeSpan _timeSpan = TimeSpan.FromMinutes(1); // Zaman aralığı (1 dəqiqə)

        public RequestLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<NoContentDto> _logger)
        {
            string requestName = context.Request.Query["name"];
            string routeName = context.Request.RouteValues["name"]?.ToString();

            

            if (string.IsNullOrEmpty(requestName))
            {
                requestName = "UnknownRequest"; // Əgər sorğu adı təyin olunmayıbsa, "UnknownRequest" olaraq təyin edin
            }

            // IP adresi və ya sorğu adını müəyyənləşdirin (IP-based və ya Request-Name based limitləmə)
            var clientKey = context.Connection.RemoteIpAddress?.ToString() ?? requestName;

            if (_requestTracker.ContainsKey(clientKey))
            {
                var (lastRequestTime, requestCount) = _requestTracker[clientKey];

                if ((DateTime.UtcNow - lastRequestTime) < _timeSpan)
                {
                    if (requestCount >= _limit)
                    {
                        // 429 Too Many Requests statusu
                        context.Response.StatusCode = 429;
                        context.Response.ContentType = "application/json";
                        _logger.LogError($"Request limit exceeded for {clientKey}. Please try again later.");
                        var result = ApiResponse<NoContentDto>.Fail($"Request limit exceeded for {clientKey}. Please try again later.", 429);
                        //var result = ApiResponse<NoContentDto>.Fail("request", $"Request limit exceeded for {clientKey}. Please try again later.", 422);

                        var options = new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // Optional: Use camelCase for JSON properties
                            WriteIndented = true // Optional: Pretty-print the JSON
                        };

                        //await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                        var data = new ObjectResult(result) { StatusCode = context.Response.StatusCode };
                        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                        return;
                    }
                    else
                    {
                        _requestTracker[clientKey] = (lastRequestTime, requestCount + 1);
                    }
                }
                else
                {
                    // Yeni zaman intervalı başlayırsa sorğu sayını sıfırlayın
                    _requestTracker[clientKey] = (DateTime.UtcNow, 1);
                }
            }
            else
            {
                // Yeni IP və ya Request-Name üçün limit izləyiçisini başlayın
                _requestTracker[clientKey] = (DateTime.UtcNow, 1);
            }

            await _next(context);
        }
    }

}

