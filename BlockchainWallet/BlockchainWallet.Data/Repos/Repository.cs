using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BlockchainWallet.Data.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> set;
        private BlockchainDbContext context;

        public Repository(BlockchainDbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public void Add(T entity)
        {
            this.set.Add(entity);
            this.context.SaveChanges();
        }

        public void Add(IEnumerable<T> entities)
        {
            this.set.AddRange(entities);
            this.context.SaveChanges();
        }

        public IEnumerable<T> All()
        {
            return this.set.ToArray();
        }
        
        public IQueryable<T> AllAsQueryable()
        {
            return this.set;
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return this.set.Where(predicate).ToArray();
        }

        public T FirstOrDefault()
        {
            return this.set.FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.set.FirstOrDefault(predicate);
        }

        public T GetById(object id)
        {
            return this.set.Find(id);
        }

        public void Update(T model)
        {
            this.context.SaveChanges();
        }

        //public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        //{
        //    return this.set.Where(predicate);
        //}

        public bool Any()
        {
            return this.set.Any();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.set.Any(predicate);
        }

        //public IdentityDbContext<User> Context => this.context;
    }
}
