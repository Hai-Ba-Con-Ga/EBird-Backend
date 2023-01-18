using EBird.Domain.Common;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        ///      Add a new entity to database
        /// </summary>
        /// <param name="entity"> New enitity</param>
        /// <returns>Number of row in database have been changed</returns>
        Task<int> CreateAsync(T entity);

        /// <summary>
        ///      Update a entity to database
        /// </summary>
        /// <param name="entity">Entity for updating</param>
        /// <returns>Number of row in database have been changed</returns>
        Task<int> UpdateAsync(T entity);

        Task<T> DeleteAsync(Guid id);

        /// <summary>
        ///     Update IsDeleted property to true for a object
        /// </summary>
        /// <param name="id">Object's id for soft delete</param>
        /// <returns>A object have soft deleted</returns>
        Task<T> DeleteSoftAsync(Guid id);
        Task<T> FindWithCondition(Expression<Func<T, bool>> predicate);
        Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
    }
}
