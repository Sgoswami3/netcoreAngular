using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse resp, string message)
        {
            resp.Headers.Add("Application-Error", message);
            resp.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
