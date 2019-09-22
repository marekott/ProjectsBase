using Microsoft.AspNet.Identity.EntityFramework;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;

namespace ProjectsBaseShared.Security
{
    public class ApplicationUserStore : UserStore<User>
    {
        public ApplicationUserStore(Context context)
            : base(context)
        {
        }
    }
}
