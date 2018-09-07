using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Src.Infrastructure.Filter
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        private HttpStatusCode MapStatusCode(Exception ex)
        {
            // Status Codes
            if (ex is ArgumentNullException)
            {
                return HttpStatusCode.NotFound;
            }
            else if (ex is ValidationException)
            {
                return HttpStatusCode.BadRequest;
            }
            else if (ex is UnauthorizedAccessException)
            {
                return HttpStatusCode.Unauthorized;
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        private readonly IHostingEnvironment _env;

        public ExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is Exception)
            {
                if (_env.IsProduction())
                {
                    context.Result = new ObjectResult(new
                    {
                        ErrorMessage = context.Exception.Message
                    });
                }
                else
                {
                    context.Result = new ObjectResult(new
                    {
                        ErrorMessage = context.Exception.Message,
                        Exception = context.Exception
                    });
                }
            
                context.HttpContext.Response.StatusCode = (int)MapStatusCode(context.Exception);

                context.Exception = null;
            }
        }
    }
}
