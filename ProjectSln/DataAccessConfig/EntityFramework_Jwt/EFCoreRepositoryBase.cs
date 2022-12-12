using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Main.DataAccessConfig.EntityFramework_Jwt
{
    public abstract class EFCoreBase<T> where T : class
    {
        private readonly IdentityContext _repository;

        public EFCoreBase(IdentityContext repository) => _repository = repository;

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
              _repository.Set<T>()
                .AsNoTracking() :
              _repository.Set<T>();

        public IQueryable<T> QueryByCondition(Expression<Func<T, bool>> expression,
                bool trackChanges) =>
                    !trackChanges ?
                        _repository.Set<T>()
                        .Where(expression)
                        .AsNoTracking() :
                        _repository.Set<T>()
                        .Where(expression);

        public void Create(T entity) => _repository.Set<T>().Add(entity);

        public void Update(T entity) => _repository.Set<T>().Update(entity);

        public void Delete(T entity) => _repository.Set<T>().Remove(entity);

        public async Task SaveChangesAsync() => await _repository.SaveChangesAsync();
    }
}