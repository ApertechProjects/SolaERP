using Microsoft.AspNetCore.Diagnostics;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Exceptions;
using System.Text.Json;

namespace SolaERP.Middlewares
{
    public static class ExceptionHandlerMiddleware
    {
        public static void UseGlobalExceptionHandlerMiddleware<T>(this IApplicationBuilder app, ILogger<T> _logger)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async handler =>
                {
                    handler.Response.ContentType = "application/json";

                    var errorFeatures = handler.Features.Get<IExceptionHandlerFeature>();
                    var statusCode = errorFeatures.Error switch
                    {
                        UserException => 400,
                        _ => 500
                    };
                    handler.Response.StatusCode = statusCode;

                    _logger.LogError(errorFeatures.Error.StackTrace);
                    var result = ApiResponse<NoContentDto>.Fail(errorFeatures.Error, 400);
                    await handler.Response.WriteAsync(JsonSerializer.Serialize(result));
                });
            });
        }

    }
}
