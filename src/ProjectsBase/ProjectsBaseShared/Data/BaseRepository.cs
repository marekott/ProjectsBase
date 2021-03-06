﻿using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ProjectsBaseShared.Data
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        protected Context Context { get; }
        private bool _disposed;

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

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Context.Dispose();
            }

            _disposed = true;
        }
    }
}
