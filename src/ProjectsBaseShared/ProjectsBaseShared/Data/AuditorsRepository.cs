using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public override List<Auditor> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
