using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync();
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetItemsAsync(string sql);
        Task<T> CreateItemAsync(T item);
        Task<T> UpdateItemAsync(string id, T item);
        Task<T> CreateOrUpdateItemAsync(T item);
        Task DeleteItemAsync(string id);
    }
}
