using AutoMapper;
using AutoMapper.Internal.Mappers;
using Shared.Core.CrossCuttingConcerns.Caching;
using Stock.BusinessLayer.Absract;
using Stock.BusinessLayer.Mapping;
using Stock.DataAccessLayer.Abstract;
using Stock.EntityLayer.Concrete;
using Stock.EntityLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.BusinessLayer.Concrete
{
    public class StockManager : IStockService
    {
        private readonly IStockDal _stockDal;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public StockManager(IStockDal stockDal, ICacheManager cacheManager, IMapper mapper)
        {
            _stockDal = stockDal;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task AddAsync(StockDto entity)
        {
            await _stockDal.AddAsync(ObjectMapper.mapper.Map<EntityLayer.Concrete.Stock>(entity));
            _cacheManager.RemoveByPattern("Stock");
        }

        public async Task DeleteAsync(string id)
        {
            await _stockDal.RemoveAsync(id);
            _cacheManager.RemoveByPattern("Stock");
        }

        public async Task<List<StockDto>> GetAllAsync()
        {
            string cacheKey = "Stock.GetAll";
            if (_cacheManager.IsAdd(cacheKey))
            {
                return _cacheManager.Get<List<StockDto>>(cacheKey); 
            }


            var values = await _stockDal.GetAllAsync();
            //var result = ObjectMapper.mapper.Map<List<StockDto>>(values);
            var result = _mapper.Map<List<StockDto>>(values);
            _cacheManager.Add(cacheKey, result,60);
            return result;
        }


        public async Task<StockDto> GetByIdAsync(string id)
        {
          string cachekey = $"Stock.GetById.{id}";

          if(_cacheManager.IsAdd(cachekey))
            {
                return _cacheManager.Get<StockDto>(cachekey);
            }

          var value = await _stockDal.GetByIdAsync(id);
          var result = _mapper.Map<StockDto>(value);

            _cacheManager.Add(cachekey, result,60);
            return result;
        }

        public async Task UpdateAsync(StockUpdateDto entity)
        {
            await _stockDal.UpdateAsync(_mapper.Map<EntityLayer.Concrete.Stock>(entity), entity.Id);

            _cacheManager.RemoveByPattern("Stock");
        }


    }
}
