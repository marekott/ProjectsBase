using System;
using System.Data.Entity;
using ProjectsBaseShared.Models;

namespace ProjectsBaseShared.Data
{
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
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
                Client = clientPzu
            };
            context.Projects.Add(projectPzu);

            var projectPko = new Project()
            {
                ProjectName = "Pko - Audit",
                ProjectStartDate = DateTime.Now,
                ProjectEndDate = DateTime.Now.AddMonths(2),
                Client = clientPko
            };
            context.Projects.Add(projectPko);

            var projectIng = new Project()
            {
                ProjectName = "Ing - Audit",
                ProjectStartDate = DateTime.Now,
                ProjectEndDate = DateTime.Now.AddMonths(2),
                Client = clientIng
            };
            context.Projects.Add(projectIng);

            context.SaveChanges();
        }
    }
}
