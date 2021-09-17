using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        private readonly ApplicationDbContext applicationDbContext; 
        
        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext; 
        }

        public Task<TEntity> Add(TEntity Entity)
        {
            applicationDbContext.Set<TEntity>().Add(Entity);
            applicationDbContext.SaveChanges();
            return null;
        }

        public Task<TEntity> Delete(int id)
        {
            var entity =  applicationDbContext.Set<TEntity>().Find(id); 
            if(entity == null)
            {
                return null; 
            }
            applicationDbContext.Set<TEntity>().Remove(entity);
            applicationDbContext.SaveChanges();
            return Task.FromResult(entity); 
        }

        public async Task<TEntity> Get(int id)
        {
            return await applicationDbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await applicationDbContext.Set<TEntity>().ToListAsync();
        }

        public  Task<TEntity> Update(TEntity Entity)
        {
            applicationDbContext.Entry(Entity).State = EntityState.Modified;
            applicationDbContext.SaveChanges();
            return null; 
        }
    }

    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        Task<T> Update(T Entity);

        Task<T> Delete(int id);

        Task<T> Add(T Entity); 

       
    }
}
