using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace ProjectsBaseWebApplication
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/SignIn"),
                Provider = new CookieAuthenticationProvider(),
                CookieSecure = CookieSecureOption.Always,
                CookieHttpOnly = true //This will instruct the browser to prevent the cookie from being accessible via JavaScript which will help prevent cross-site scripting attacks.
            });
        }
    }
}