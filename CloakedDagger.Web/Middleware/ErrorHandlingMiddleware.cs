using System;
using System.Net;
using System.Threading.Tasks;
using CloakedDagger.Common.Exceptions;
using DasCookbook.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace CloakedDagger.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {

        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is EntityAlreadyExistsException)
            {
                var result = new
                {
                    error = exception.Message
                };
                return WriteErrorToResponse(context, HttpStatusCode.BadRequest, result);
            }

            if (exception is EntityNotFoundException)
            {
                var result = new
                {
                    error = exception.Message
                };
                return WriteErrorToResponse(context, HttpStatusCode.NotFound, result);
            }
            if (exception is EntityValidationException validationException)
            {
                var result = new
                {
                    message = String.IsNullOrEmpty(validationException.Message) ? "Validation issues exist." : validationException.Message,
                    validationErrors = validationException.ValidationResults
                };
                return WriteErrorToResponse(context, HttpStatusCode.BadRequest, result);
            }
            _logger.Error(exception, "Error occurred processing request.");

            return WriteErrorToResponse(context, HttpStatusCode.InternalServerError, new
            {
                //TODO: Maybe change this message / behaviour  based upon a setting / environment
                message = "An error occured while processing this request"
            });
        }

        private Task WriteErrorToResponse(HttpContext context, HttpStatusCode code, object result)
        {
        
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

    }
}