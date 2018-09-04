using System;
using System.Collections.Generic;

namespace InstagramClone.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        List<T> ListAll();
        List<T> List(Func<T, bool> predicate);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
