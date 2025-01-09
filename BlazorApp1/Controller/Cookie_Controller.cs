using Microsoft.AspNetCore.Http;

namespace BlazorApp1.Controller
{
    public class Cookie_Controller
    {
        public IHttpContextAccessor _http_Context_Accessor;

        public Cookie_Controller(IHttpContextAccessor http_Context_Accessor) 
        {
            _http_Context_Accessor = http_Context_Accessor;
        }

        public void Set_Cookie(string username)
        {
            _http_Context_Accessor.HttpContext?.Response.Cookies.Append("AuthCookie", username, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Set to true if using HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7) // Example expiration
            });
        }

        public void Delete_Cookie()
        {
            _http_Context_Accessor.HttpContext?.Response.Cookies.Delete("AuthCookie");
        }
    }
}
