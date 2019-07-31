using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ProjectsBaseShared.Data
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        protected Context Context { get; }

        protected BaseRepository(Context context)
        {
            Context = context;
        }

        public abstract TEntity Get(Guid guid, bool includeRelatedEntities = true);
        public abstract List<TEntity> GetList();

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        public void Edit(TEntity entity)
        {
            throw new NotImplementedException();
            //Context.Entry(entity).State = EntityState.Modified;
            //Context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
;       }
    }
}
