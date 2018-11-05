using GlobalExceptionHandler.WebApi;
using Newtonsoft.Json;
using PaymentService.Api.Exceptions;
using System.Net;

namespace PaymentService.Configuration
{
    public static class ExceptionMapper
    {
        public static void MapExceptions(this ExceptionHandlerConfiguration cfg)
        {
            cfg.ContentType = "application/json";

            cfg.ResponseBody(s => JsonConvert.SerializeObject(new
            {
                Message = s is BussinesExceptions ? s.Message : "An error occured whilst processing your request"
            }));

            cfg.Map<FluentValidation.ValidationException>()
                .ToStatusCode(HttpStatusCode.BadRequest)
                .WithBody((ex, ctx) => SerializeValidationException(ex));
        }

        private static string SerializeValidationException(FluentValidation.ValidationException ex)
        {
            return JsonConvert.SerializeObject(new
            {
                Code = "400",
                Message = ex.Errors.ToString()
            });
        }
    }
}
