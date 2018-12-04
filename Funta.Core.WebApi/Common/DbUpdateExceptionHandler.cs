using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funta.Core.Web.Api.Common
{
    public class DbUpdateExceptionHandler
    {
        private readonly RequestDelegate @_next;

        public DbUpdateExceptionHandler(RequestDelegate @next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DbUpdateException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, DbUpdateException ex)
        {
            object result;
            var code = 400;
            if (ex.InnerException.Message.ToLower().Contains("duplicate"))
            {
                code = 409;
                result = new
                {
                    Error = "Error updating database. Duplicate value."
                };
            }
            else
            {
                result = new
                {
                    Error = "Error updating database."
                };
            }
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = code;
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }

    public static class HandleDbUpdateExceptionExtensions
    {
        public static IApplicationBuilder UseDbUpdateExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbUpdateExceptionHandler>();
        }
    }
}
