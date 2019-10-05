using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectsBaseShared.Models;
using ProjectsBaseShared.Security;

namespace ProjectsBaseShared.Data
{
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
#if DEBUG
        protected override void Seed(Context context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new ApplicationUserManager(userStore);

            var userMarian = new User
            {
                UserName = "mariantestowy@eac.com",
                Email = "mariantestowy@eac.com"
            };

            userManager.Create(userMarian, "marian123");

            var userJan = new User
            {
                UserName = "jankowalski@eac.com",
                Email = "jankowalski@eac.com"
            };

            userManager.Create(userJan, "jan123");

            var auditorMarian = new Auditor()
            {
                AuditorName = "Marian",
                AuditorSurname = "Testowy",
                AuditorId = Guid.Parse(userMarian.Id)
            };

            context.Auditors.Add(auditorMarian);

            var auditorJan = new Auditor()
            {
                AuditorName = "Jan",
                AuditorSurname = "Kowalski",
                AuditorId = Guid.Parse(userJan.Id)
            };

            context.Auditors.Add(auditorJan);

            var clientPzu = new Client()
            {
                ClientName = "PZU"
            };

            var clientPko = new Client()
            {
                ClientName = "Pko"
            };

            var clientIng = new Client()
            {
                ClientName = "Ing"
            };

            var projectPzu = new Project()
            {
                ProjectName = "PZU - Audit",
                ProjectStartDate = DateTime.Now,
                ProjectEndDate = DateTime.Now.AddMonths(2),
                Client = clientPzu,
                UserId = userMarian.Id,
                User = userMarian
            };
            context.Projects.Add(projectPzu);

            var projectPko = new Project()
            {
                ProjectName = "Pko - Audit",
                ProjectStartDate = DateTime.Now,
                ProjectEndDate = DateTime.Now.AddMonths(2),
                Client = clientPko,
                UserId = userMarian.Id,
                User = userMarian
            };
            context.Projects.Add(projectPko);

            var projectIng = new Project()
            {
                ProjectName = "Ing - Audit",
                ProjectStartDate = DateTime.Now,
                ProjectEndDate = DateTime.Now.AddMonths(2),
                Client = clientIng,
                UserId = userJan.Id,
                User = userJan
            };
            context.Projects.Add(projectIng);

            context.SaveChanges();
        }
#endif
    }
}
