using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using ProjectsBaseShared.Data;
using ProjectsBaseShared.Models;
using ProjectsBaseShared.Security;
using ProjectsBaseWebApplication.Models;

namespace ProjectsBaseWebApplication
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ProjectsRepository>().As<IRepository<Project>>();
            builder.RegisterType<AuditTeamRepository>().As<IRepository<AuditTeam>>();
            builder.RegisterType<AuditorsRepository>().As<IRepository<Auditor>>();
            builder.RegisterType<ClientsRepository>().As<IRepository<Client>>();
            builder.RegisterType<ProjectValidator>().As<IValidator<Project>>();
            builder.RegisterType<Context>();

            builder.RegisterType<ApplicationUserStore>().As<IUserStore<User>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
