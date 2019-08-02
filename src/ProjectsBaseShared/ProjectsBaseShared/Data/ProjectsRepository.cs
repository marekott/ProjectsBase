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
            return includeRelatedEntities ?
                Context.Projects.GetRelatedEntities()
                    .SingleOrDefault(p => p.ProjectId == guid) :
                Context.Projects
                    .SingleOrDefault(p => p.ProjectId == guid);
        }

        public override List<Project> GetList()
        {
            return Context.Projects
                .GetRelatedEntities()
                .ToList();
        }
    }

    public static class ProjectIQueryableExtension
    {
        public static IQueryable<Project> GetRelatedEntities(this IQueryable<Project> collection)
        {
            return collection
                .Include(p => p.Client)
                .Include(p => p.Auditors.Select(a => a.Auditor));
        }
    }
}
