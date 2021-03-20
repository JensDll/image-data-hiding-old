using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAPI.Extensions
{
    public static class HttpContextExtension
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            return httpContext.User == null
                ? default
                : int.Parse(httpContext.User.FindFirst("userId").Value);
        }

        public static string GetUsername(this HttpContext httpContext)
        {
            return httpContext.User == null
                ? default
                : httpContext.User.FindFirst("username").Value;
        }
    }
}
