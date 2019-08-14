using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ProjectsBaseShared.Models;

namespace ProjectsBaseShared.Data
{
    public class AuditorsRepository : BaseRepository<Auditor>
    {
        public AuditorsRepository(Context context) : base(context)
        {
        }

        public override Auditor Get(Guid guid, bool includeRelatedEntities = true)
        {
            return includeRelatedEntities ? 
                Context.Auditors.GetRelatedEntities()
                    .SingleOrDefault(a => a.AuditorId == guid) : 
                Context.Auditors
                    .SingleOrDefault(a => a.AuditorId == guid);
        }

        public override List<Auditor> GetList()
        {
            return Context.Auditors
                .GetRelatedEntities()
                .ToList();
        }
    }
    public static class AuditorIQueryableExtension
    {
        public static IQueryable<Auditor> GetRelatedEntities(this IQueryable<Auditor> collection)
        {
            return collection
                .Include(c => c.Projects)
                .Include(c => c.Projects.Select(p => p.Project.Client));
        }
    }
}
