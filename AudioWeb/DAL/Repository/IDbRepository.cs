using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public interface IDbRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);
        TEntity GetById(long id);
        TEntity Get(Expression<Func<TEntity, bool>> where);
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);
        IEnumerable<TEntity> GetAll();
    }
}
