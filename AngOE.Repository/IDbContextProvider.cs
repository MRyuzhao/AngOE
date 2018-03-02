using System;
using AngOE.Data;
using Castle.Windsor;

namespace AngOE.Repository
{
    public interface IDbContextProvider : IDisposable
    {
        AngOeDbContext GetAngOeDbContext();
    }

    public class Disposable: IDisposable
    {
        private bool _isDisposed;

        ~Disposable()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore();
            }

            _isDisposed = true;
        }
        protected virtual void DisposeCore()
        {
        }
    }

    public class DbContextProvider : Disposable, IDbContextProvider
    {
        private readonly IWindsorContainer _container;
        public DbContextProvider(IWindsorContainer container)
        {
            _container = container;
        }

        public AngOeDbContext GetAngOeDbContext()
        {
            return _container.Resolve<AngOeDbContext>();
        }
    }
}