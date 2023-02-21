using System;
using System.Net;
using System.Threading.Tasks;
using CottageApi.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CottageApi.Middlewares
{
    public class GeneralExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GeneralExceptionMiddleware> _logger;

        public GeneralExceptionMiddleware(RequestDelegate next, ILogger<GeneralExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var modelState = new ModelStateDictionary();

                foreach (var error in ex.Errors)
                {
                    modelState.AddModelError(error.FieldName, error.Message);
                }

                await HandleException(context.Response, LogLevel.Warning, ex, HttpStatusCode.UnprocessableEntity, new SerializableError(modelState));
            }
            catch (Exception ex)
            {
                await HandleException(context.Response, LogLevel.Error, ex, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private Task HandleException(HttpResponse response, LogLevel logLevel, Exception exception, HttpStatusCode statusCode, object responseToWrite = null)
        {
            if (statusCode != HttpStatusCode.UnprocessableEntity)
            {
                _logger.Log(logLevel, exception, exception.Message);
            }

            response.StatusCode = (int)statusCode;
            response.Headers.Add("X-Status-Reason", "Validation error");

            return responseToWrite != null ?
                response.WriteAsync(
                    JsonConvert.SerializeObject(
                        responseToWrite,
                        Formatting.None,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }))
                : Task.CompletedTask;
        }
    }
}