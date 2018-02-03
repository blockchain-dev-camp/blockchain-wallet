using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BlockchainWallet.Data.Repos
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Add(IEnumerable<T> entities);
        void Update(T model);
        T FirstOrDefault();
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T GetById(object id);
        bool Any();
        bool Any(Expression<Func<T, bool>> predicate);
        IQueryable<T> AllAsQueryable();
        IEnumerable<T> All();
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        //IdentityDbContext<User> Context { get; }
    }
}
