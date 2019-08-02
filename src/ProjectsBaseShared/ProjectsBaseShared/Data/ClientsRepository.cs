using System;
using System.Collections.Generic;
using ProjectsBaseShared.Models;

namespace ProjectsBaseShared.Data
{
    public class ClientsRepository : BaseRepository<Client>
    {
        public ClientsRepository(Context context) : base(context)
        {
        }

        public override Client Get(Guid guid, bool includeRelatedEntities = true)
        {
            throw new NotImplementedException();
        }

        public override List<Client> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
