using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlazorApp1.Controller
{
    public class Authentication_Controller
    {
        private readonly RequestDelegate _next;
        private readonly Cookie_Service _cookieService;

        public Authentication_Controller(RequestDelegate next, Cookie_Service cookie_Service)
        {
            _next = next;
            _cookieService = cookie_Service;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the cookie exists
            var cookie = context.Request.Cookies["UserSession"];
            if(string.IsNullOrEmpty(cookie) && !context.Request.Path.StartsWithSegments("/login_Page") && !context.Request.Path.StartsWithSegments("/user_Register"))
            {
                // If the cookie doesn't exist and the request isn't for the login page or Register, redirect to login
                context.Response.Redirect("/login_Page");
                return;
            }

            await _next(context);
        }
    }
}
