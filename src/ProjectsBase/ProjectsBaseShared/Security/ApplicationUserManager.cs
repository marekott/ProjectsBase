using Microsoft.AspNet.Identity;
using ProjectsBaseShared.Models;

namespace ProjectsBaseShared.Security
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> userStore) : base(userStore)
        {
        }
    }
}
