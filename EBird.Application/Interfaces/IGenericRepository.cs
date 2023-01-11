using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task CreateAsync (T entity);
        Task<List<T>> GetAllAsync ();
        
        Task<T> GetByIdAsync (Guid id);
        Task UpdateAsync(T entity);
        
        Task<T> DeleteAsync (Guid id);
        Task<T> DeleteSoftAsync(Guid id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<IList<T>> WhereAsync (Expression<Func<T, bool>> predicate, params string[] navigationProperties);


    }
}
