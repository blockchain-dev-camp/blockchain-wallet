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
            set = context.Set<T>();
        }

        public void Add(T entity)
        {
            set.Add(entity);
            context.SaveChanges();
        }

        public void Add(IEnumerable<T> entities)
        {
            set.AddRange(entities);
            context.SaveChanges();
        }

        public IEnumerable<T> All()
        {
            return set.ToArray();
        }
        
        public IQueryable<T> AllAsQueryable()
        {
            return set;
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return set.Where(predicate).ToArray();
        }

        public T FirstOrDefault()
        {
            return set.FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return set.FirstOrDefault(predicate);
        }

        public T GetById(object id)
        {
            return set.Find(id);
        }

        public void Update(T model)
        {
            context.SaveChanges();
        }

        //public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        //{
        //    return this.set.Where(predicate);
        //}

        public bool Any()
        {
            return set.Any();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return set.Any(predicate);
        }

        //public IdentityDbContext<User> Context => this.context;
    }
}
