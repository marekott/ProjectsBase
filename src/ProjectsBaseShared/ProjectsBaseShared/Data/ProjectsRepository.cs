using System;
using System.Collections.Generic;
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
            var project = Context.Projects.AsQueryable();

            if (includeRelatedEntities)
            {
                throw new NotImplementedException();
            }

            return project
                .SingleOrDefault(p => p.ProjectId == guid);
        }

        public override List<Project> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
