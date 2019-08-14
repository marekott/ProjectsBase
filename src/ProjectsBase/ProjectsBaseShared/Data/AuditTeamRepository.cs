using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ProjectsBaseShared.Models;

namespace ProjectsBaseShared.Data
{
    public class AuditTeamRepository : BaseRepository<AuditTeam>
    {
        public AuditTeamRepository(Context context) : base(context)
        {
        }

        public override AuditTeam Get(Guid guid, bool includeRelatedEntities = true)
        {
            return includeRelatedEntities
                ? Context.AuditTeams.GetRelatedEntities()
                    .SingleOrDefault(at => at.Id == guid)
                : Context.AuditTeams
                    .SingleOrDefault(at => at.Id == guid);
        }

        public override List<AuditTeam> GetList()
        {
            throw new NotImplementedException();
        }
    }

    public static class AuditTeamIQueryableExtension
    {
        public static IQueryable<AuditTeam> GetRelatedEntities(this IQueryable<AuditTeam> collection)
        {
            return collection
                .Include(at => at.Project)
                .Include(at => at.Auditor);
        }
    }
}
