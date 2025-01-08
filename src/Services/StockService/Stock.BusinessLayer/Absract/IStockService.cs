using Microsoft.VisualBasic;
using Stock.EntityLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.BusinessLayer.Absract
{
    public interface IStockService
    {
        Task<List<StockDto>> GetAllAsync();
        Task<StockDto> GetByIdAsync(string id);
        Task AddAsync(StockDto entity);
        Task UpdateAsync(StockUpdateDto entity);
        Task DeleteAsync(string id);
    }
}
