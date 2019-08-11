using System;
using System.Collections.Generic;

namespace ProjectsBaseShared.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(Guid guid, bool includeRelatedEntities = true);
        List<TEntity> GetList();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
