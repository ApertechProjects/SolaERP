using Microsoft.AspNetCore.Diagnostics;
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
                config.Run( async handler =>
                {
                    handler.Response.ContentType = "application/json";

                    var errorFeatures = handler.Features.Get<IExceptionHandlerFeature>();

                    var result = ApiResponse<NoContentDto>.Fail(errorFeatures.Error.Message, 500);

                    await handler.Response.WriteAsync(JsonSerializer.Serialize(result));

                });            
            });
        }

    }
}
