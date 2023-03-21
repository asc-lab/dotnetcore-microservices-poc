using System.Net;
using FluentValidation;
using GlobalExceptionHandler.WebApi;
using Newtonsoft.Json;

namespace PricingService.Configuration;

public static class ExceptionMapper
{
    public static void MapExceptions(this ExceptionHandlerConfiguration cfg)
    {
        cfg.ContentType = "application/json";

        cfg.ResponseBody(s => JsonConvert.SerializeObject(new
        {
            Message = "An error occured whilst processing your request"
        }));


        cfg.Map<ValidationException>()
            .ToStatusCode(HttpStatusCode.BadRequest)
            .WithBody((ex, ctx) => SerializeValidationException(ex));
    }

    private static string SerializeValidationException(ValidationException ex)
    {
        return JsonConvert.SerializeObject(new
        {
            Code = "400",
            Message = ex.Errors.ToString()
        });
    }
}