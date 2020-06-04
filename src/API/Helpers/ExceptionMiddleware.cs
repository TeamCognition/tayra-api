using System;
using System.IO;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Tayra.API.Helpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if(exception is PropertyValidationException)
            {
                var x = exception as PropertyValidationException;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(x.PropertyValidationErrors));
            }

            if (exception is CogSecurityException)
            {
                FillRequestData(exception as SecurityException, context);
                return context.Response.WriteAsync(exception.Message); //TODO: don't show to user
            }

            if (exception is ApplicationException)
            {
                var x = exception as ApplicationException;
                return context.Response.WriteAsync(x.Message);
            }

            return context.Response.WriteAsync(new { Message = "Errorino :)" }.ToString());
        }

        private void FillRequestData(SecurityException e, HttpContext context)
        {
            if (context?.User != null)
            {
                var principal = new CogPrincipal(context.User);
                e.Data.Add("ProfileId", principal.ProfileId);//TODO: add more
            }

            e.Url = context.Request.GetRawTarget();
            e.Data.Add("RequestQueryString", JsonConvert.SerializeObject(context.Request.QueryString));

            if (context.Request.Method == "POST"
                && context.Request.ContentLength <= 1024
                && (context.Request.ContentType.Contains("json")
                    || context.Request.ContentType.Contains("form-urlencoded")))
                    
            {
                context.Request.Body.Seek(0L, SeekOrigin.Begin);
                using (StreamReader streamReader = new StreamReader(context.Request.Body))
                {
                    e.Data.Add("RequestBody", streamReader.ReadToEnd());
                }
                
            }
        }
    }
}
