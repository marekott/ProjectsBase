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
            var auditors = Context.Auditors.AsQueryable();

            if (includeRelatedEntities)
            {
                auditors = auditors
                    .Include(a => a.Projects)
                    .Include(a => a.Projects.Select(p => p.Project.Client));
            }

            return auditors
                .SingleOrDefault(a => a.AuditorId == guid);
        }

        public override List<Auditor> GetList()
        {
            return Context.Auditors
                .Include(a => a.Projects)
                .Include(a => a.Projects.Select(p => p.Project.Client))
                .ToList();
        }
    }
}
