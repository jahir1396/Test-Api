using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Proyecto
{
    public interface IRepository<T> where T : class
    {        
        List<T> GetAllList();
        Task<List<T>> GetAllListAsync();
        List<T> GetAllList(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T Get(params object?[]? id);
        Task<T> GetAsync(params object?[]? id);
        T Load(object id);
        void Insert(T entity);
        Task InsertAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        int Count();
        Task<int> CountAsync();
        int Count(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
