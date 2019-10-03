using BlumindApp.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace BlumindApp.Filters {
    public class GlobalErrorFilter : IExceptionFilter {
        public void OnException(ExceptionContext context)
        {
            var status = HttpStatusCode.InternalServerError;
            var errorType = context.Exception.GetType();
            var error = new ErrorResult();

            if (errorType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Unauthorized;
                error.Message = "Unauthorized access";

            }
            else
            {
                error.Message = context.Exception.Message;
                error.InnerMessage = context.Exception.InnerException?.Message;
            }

            context.ExceptionHandled = true;
            var response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";

            response.WriteJsonAsync(error);
        }
    }
}
