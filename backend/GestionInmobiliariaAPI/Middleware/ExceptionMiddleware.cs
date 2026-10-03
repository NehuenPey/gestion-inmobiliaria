using System.Net;
using System.Text.Json;

namespace GestionInmobiliariaAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ocurrió una excepción no controlada."
                );

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType =
                    "application/json";

                var respuesta = new
                {
                    mensaje =
                        "Ocurrió un error interno en el servidor."
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(respuesta)
                );
            }
        }
    }
}