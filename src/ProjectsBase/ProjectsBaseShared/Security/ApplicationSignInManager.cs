using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProjectsBaseShared.Models;

namespace ProjectsBaseShared.Security
{
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }
    }
}
