using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AngOE.Repository
{
    public interface IBaseRepository<T> where T:class
    {
        T Add(T t);

        void Delete(int id);

        void Update(T t);

        T Get(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>>filter);

        IEnumerable<T> GetPagingData(Expression<Func<T, bool>>filter, int pageIndex,
            int pageSize, string orderByPropertyName, bool isAsc, out int totleRecord);
    }
}