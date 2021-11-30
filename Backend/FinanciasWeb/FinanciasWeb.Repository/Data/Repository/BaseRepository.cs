using FinanciasWeb.Domain.Interfaces;
using FinanciasWeb.Repository.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanciasWeb.Repository.Data.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly MySqlContext _mySqlContext;

        public BaseRepository(MySqlContext mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _mySqlContext.Set<TEntity>().AsNoTracking();
        }

        public void Insert(TEntity obj)
        {
            _mySqlContext.Set<TEntity>().Add(obj);
            _mySqlContext.SaveChanges();
        }

        public void Update(TEntity obj)
        {
            _mySqlContext.Entry(obj).State = EntityState.Modified;
            _mySqlContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _mySqlContext.Set<TEntity>().Remove(Select(id));
            _mySqlContext.SaveChanges();
        }

        public IList<TEntity> Select() =>
            _mySqlContext.Set<TEntity>().AsNoTracking().ToList();

        public TEntity Select(int id) =>
            _mySqlContext.Set<TEntity>().Find(id);
    }
}
