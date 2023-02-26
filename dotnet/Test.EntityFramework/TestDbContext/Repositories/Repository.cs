using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Proyecto
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }        
        

        public virtual List<T> GetAllList()
        {
            return _unitOfWork.Context.Set<T>().ToList();
        }

        public virtual Task<List<T>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public virtual List<T> GetAllList(Expression<Func<T,bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate).ToList();
        }

        public virtual Task<List<T>> GetAllListAsync(Expression<Func<T,bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual Task<T> FirstOrDefaultAsync(Expression<Func<T,bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public virtual T Get(params object?[]? id)
        {
            var entity = _unitOfWork.Context.Set<T>().Find(id);
            if (entity == null)
            {
                throw new Exception($"No existe una instandica {entity} con id = {id}");
            }

            return entity;
        }

        public virtual Task<T> GetAsync(params object?[]? id)
        {
            return Task.FromResult(Get(id));
        }

        public virtual T Load(object id)
        {
            return Get(id);
        }

        public virtual void Insert(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);            
        }

        public virtual async Task InsertAsync(T entity)
        {
            await _unitOfWork.Context.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _unitOfWork.Context.Set<T>().Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _unitOfWork.Context.Set<T>().Remove(entity);
        }

        public virtual void Delete(Expression<Func<T,bool>> predicate)
        {
            foreach (var entity in GetAllList(predicate))
            {
                Delete(entity);
            }
        }

        public virtual int Count()
        {
            return GetAllList().Count();
        }
        
        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Count(predicate);
        }

        public virtual Task<int> CountAsync(Expression<Func<T,bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        //public virtual async Task DeleteAsync(T entity)
        //{
        //    Delete(entity);
        //    return Task.CompletedTask;
        //}

        //public virtual async Task UpdateAsync(T entity)
        //{
        //    await _unitOfWork.Context.Set<T>().Update(entity);
        //}

        //public virtual object InsertAndGetId(T entity)
        //{
        //    var mEntity = _unitOfWork.Context.Set<T>().Add(entity);
        //    return mEntity.;
        //}


    }
}
