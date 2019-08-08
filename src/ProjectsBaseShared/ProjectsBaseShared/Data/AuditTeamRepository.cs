using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public override List<AuditTeam> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
