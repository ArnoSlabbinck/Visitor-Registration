using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<TEntity> logger;
        
        public BaseRepository(ApplicationDbContext applicationDbContext, ILogger<TEntity> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        public Task<TEntity> Add(TEntity Entity)
        {
            applicationDbContext.Set<TEntity>().Add(Entity);
            logger.LogInformation($"A new {Entity} is created in the database");
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
            logger.LogInformation($"{entity.ToString()} is now deleted from the database with id of {id}");
            applicationDbContext.SaveChanges();
            return Task.FromResult(entity); 
        }

        public async Task<TEntity> Get(int id)
        {
            return await applicationDbContext.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return applicationDbContext.Set<TEntity>().AsQueryable();
        }

        public  Task<TEntity> Update(TEntity Entity)
        {
            applicationDbContext.Entry(Entity).State = EntityState.Modified;
            logger.LogInformation($"{Entity.ToString()} is now updated int the database ");
            applicationDbContext.SaveChanges();
            return null; 
        }
    }

    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        Task<T> Get(int id);

        Task<T> Update(T Entity);

        Task<T> Delete(int id);

        Task<T> Add(T Entity); 

       
    }
}
