using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using DAL;

namespace DAL.Repository
{
  public class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
  {
    private WebPlayerDbContext _db;
    private readonly IDbSet<TEntity> dbset;

    public DbRepository(IDbFactory databaseFactory)
    {
      DatabaseFactory = databaseFactory;
      dbset = DataContext.Set<TEntity>();
    }

    protected IDbFactory DatabaseFactory { get; private set; }

    protected WebPlayerDbContext DataContext => _db ?? (_db = DatabaseFactory.Get());

    public virtual void Add(TEntity entity)
    {
      dbset.Add(entity);
      _db.SaveChanges();
    }

    public virtual void Update(TEntity entity)
    {
      _db.Entry(entity).State = EntityState.Modified;
      _db.SaveChanges();
    }

    public virtual void Delete(TEntity entity)
    {
      dbset.Attach(entity);
      dbset.Remove(entity);
      _db.SaveChanges();
    }

    public virtual void Delete(Expression<Func<TEntity, bool>> where)
    {
      IEnumerable<TEntity> objects = dbset.Where<TEntity>(where).AsEnumerable();
      foreach (TEntity obj in objects)
        dbset.Remove(obj);
      _db.SaveChanges();
    }

    public virtual TEntity GetById(long id)
    {
      return dbset.Find(id);
    }

    public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
    {
      return dbset.Where(where).FirstOrDefault<TEntity>();
    }

    public virtual IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
    {
      return dbset.Where(where).ToList();
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
      return dbset.ToList();
    }

  }
}
