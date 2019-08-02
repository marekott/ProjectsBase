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
            return includeRelatedEntities ?
                Context.Clients.GetRelatedEntities()
                    .SingleOrDefault(c => c.ClientId == guid) :
                Context.Clients
                    .SingleOrDefault(c => c.ClientId == guid);
        }

        public override List<Client> GetList()
        {
            return Context.Clients
                .GetRelatedEntities()
                .ToList();
        }
    }

    public static class ClientIQueryableExtension
    {
        public static IQueryable<Client> GetRelatedEntities(this IQueryable<Client> collection)
        {
            return collection
                .Include(c => c.Projects)
                .Include(c => c.Projects.Select(p => p.Auditors));
        }
    }
}
