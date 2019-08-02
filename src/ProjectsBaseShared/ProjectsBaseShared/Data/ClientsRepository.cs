using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            var clients = Context.Clients.AsQueryable();

            if (includeRelatedEntities)
            {
                clients = clients
                    .Include(c => c.Projects)
                    .Include(c => c.Projects.Select(p => p.Auditors));
            }

            return clients.SingleOrDefault(c => c.ClientId == guid);
        }

        public override List<Client> GetList()
        {
            return Context.Clients
                .Include(c => c.Projects)
                .Include(c => c.Projects.Select(p => p.Auditors))
                .ToList();
        }
    }
}
