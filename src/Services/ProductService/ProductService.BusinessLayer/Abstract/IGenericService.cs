using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.BusinessLayer.Abstract
{
    public interface IGenericService<TResult, TUpdate, TCreate>
    {
        Task<List<TResult>> GetAllAsync();
        Task<TResult> GetByIdAsync(string id);
        Task AddAsync(TCreate entity);
        Task UpdateAsync(TUpdate entity);
        Task DeleteAsync(string id);
    }
}
