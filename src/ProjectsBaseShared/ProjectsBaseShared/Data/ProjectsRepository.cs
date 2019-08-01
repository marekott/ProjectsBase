using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ProjectsBaseShared.Models;

namespace ProjectsBaseShared.Data
{
    public class ProjectsRepository : BaseRepository<Project>
    {
        public ProjectsRepository(Context context) : base(context)
        {
        }

        public override Project Get(Guid guid, bool includeRelatedEntities = true)
        {
            var projects = Context.Projects.AsQueryable();

            if (includeRelatedEntities)
            {
                projects = projects
                    .Include(p => p.Client)
                    .Include(p => p.Auditors.Select(a => a.Auditor));
            }

            return projects
                .SingleOrDefault(p => p.ProjectId == guid);
        }

        public override List<Project> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
