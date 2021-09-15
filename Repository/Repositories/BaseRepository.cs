using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        private ApplicationDbContext applicationDbContext; 
        
        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext; 
        }

        public async Task<TEntity> Add(TEntity Entity)
        {
            applicationDbContext.Set<TEntity>().Add(Entity);
            await applicationDbContext.SaveChangesAsync();
            return Entity;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await applicationDbContext.Set<TEntity>().FindAsync(); 
            if(entity == null)
            {
                return entity; 
            }
            applicationDbContext.Set<TEntity>().Remove(entity);
            return entity; 
        }

        public async Task<TEntity> Get(int id)
        {
            return await applicationDbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await applicationDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Update(TEntity Entity)
        {
            applicationDbContext.Entry(Entity).State = EntityState.Modified;
            await applicationDbContext.SaveChangesAsync();
            return Entity; 
        }
    }

    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAll();

        Task<T> Get(int id);

        Task<T> Update(T Entity);

        Task<T> Delete(int id);

        Task<T> Add(T Entity); 

       
    }
}
