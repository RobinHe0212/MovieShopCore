﻿using Microsoft.EntityFrameworkCore;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Repositories
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly MovieShopDbContext _dbContext;
        public EfRepository(MovieShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)

                return await _dbContext.Set<T>().Where(filter).CountAsync();

            return await _dbContext.Set<T>().CountAsync();
        }

        public Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public Task<IEnumerable<T>> ListAllWithIncludesAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
