

using System.Net;
using Aplicacion.ManejadorError;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErrorMiddleware> _logger;
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {

            try
            {

                await _next(context);

            }
            catch (Exception ex)
            {
                await ManejadorExcepctionAsincrono(context, ex, _logger);
            }
        }

        private async Task ManejadorExcepctionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            object? errores = null;
            switch (ex)
            {
                case ManejadorExcepcion me:
                    logger.LogError(ex, "Manejador Error");
                    errores = me.Errores;
                    context.Response.StatusCode = (int)me.Codigo;
                    break;
                case Exception e:
                    logger.LogError(ex, "Error de Servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if(errores != null){
                    var resultado = JsonConvert.SerializeObject(new {errores});
                    await context.Response.WriteAsync(resultado);
            }
        }
    }
}