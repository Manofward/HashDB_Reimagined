using Microsoft.AspNetCore.Http;
using System;

namespace BlazorApp1.Controller
{
    public class Cookie_Service
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Cookie_Service(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Add_Cookie(string key, string value, int daysExpiry = 1)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Helps mitigate XSS
                    Secure = true,   // Set to true if using HTTPS
                    Expires = DateTimeOffset.UtcNow.AddDays(daysExpiry), // Cookie expiration
                    Path = "/",      // Set the path for the cookie
                    SameSite = SameSiteMode.Strict // Optional, to mitigate CSRF
                };

                // Adding the cookie to the response
                context.Response.Cookies.Append(key, value, cookieOptions);
            } 
        }   
    }
}
