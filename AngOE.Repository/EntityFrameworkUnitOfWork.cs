using System;
using System.Data.Entity;
using AngOE.Repository.UnitOfWork;

namespace AngOE.Repository
{
    public class EntityFrameworkUnitOfWork: IUnitOfWork
    {
        private DbContext _dbContext;

        public EntityFrameworkUnitOfWork(IDbContextProvider dbContext)
        {
            _dbContext = dbContext.GetAngOeDbContext();
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}