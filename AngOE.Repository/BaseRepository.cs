using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AngOE.Common;
using AngOE.Data;

namespace AngOE.Repository
{
    public abstract class BaseRepository<T> where T : class
    {
        private readonly IDbContextProvider _dbContextProvider;
        protected AngOeDbContext DbContext => _dbContextProvider.GetAngOeDbContext();

        protected BaseRepository(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public T Add(T t)
        {
            DbContext.Set<T>().Add(t);
            SafeSaveChanges();
            return t;
        }

        public void Delete(int id)
        {
            var t = Get(id);
            DbContext.Set<T>().Remove(t);
            SafeSaveChanges();
        }

        public void Update(T t)
        {
            var entry = DbContext.Entry(t);
            DbContext.Set<T>().Attach(t);
            entry.State = EntityState.Modified;
            SafeSaveChanges();
        }

        public T Get(int id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return DbContext.Set<T>().ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            return DbContext.Set<T>().Where(filter).ToList();
        }

        public IEnumerable<T> GetPagingData(Expression<Func<T,bool>>filter,
            int pageIndex,int pageSize,string orderByPropertyName,bool isAsc,
            out int totalRecordes)
        {
            var source = DbContext.Set<T>().Where(filter);
            totalRecordes = source.Count();
            return source.SortByProperty(orderByPropertyName, isAsc)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .AsQueryable().ToList();
        }

        private void SafeSaveChanges()
        {
            foreach (var error in DbContext.GetValidationErrors())
            {
                var entityType = error.Entry.Entity.GetType().BaseType;

                foreach (var validationError in error.ValidationErrors)
                {
                    var property = entityType.GetProperty(validationError.PropertyName);
                    if (property.GetCustomAttributes(typeof(RequiredAttribute), true).Any())
                    {
                        property.GetValue(error.Entry.Entity, null);
                    }
                }
            }

            DbContext.SaveChanges();
        }
    }
}