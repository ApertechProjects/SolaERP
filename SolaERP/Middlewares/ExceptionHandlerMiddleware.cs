using Microsoft.AspNetCore.Diagnostics;
using SolaERP.Application.Exceptions;
using SolaERP.Infrastructure.Dtos;
using System.Text.Json;

namespace SolaERP.Middlewares
{
    public static class ExceptionHandlerMiddleware
    {
        public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app)
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
                    var result = ApiResponse<NoContentDto>.Fail(errorFeatures.Error.Message, 200);
                    await handler.Response.WriteAsync(JsonSerializer.Serialize(result));

                });
            });
        }

    }
}
