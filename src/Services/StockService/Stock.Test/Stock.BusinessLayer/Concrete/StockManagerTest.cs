using AutoMapper;
using FakeItEasy;
using NLog.Web.Enums;
using Shared.Core.CrossCuttingConcerns.Caching;
using Stock.BusinessLayer.Concrete;
using Stock.BusinessLayer.Mapping;
using Stock.DataAccessLayer.Abstract;
using Stock.EntityLayer.Concrete;
using Stock.EntityLayer.Dtos;
using Stock.Test.FakeDataGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Test.Stock.BusinessLayer.Concrete
{
    [TestFixture]
    public class StockManagerTest
    {
        private IStockDal _stockDal;
        private ICacheManager _cacheManager;
        private StockManager _stockManager;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _stockDal = A.Fake<IStockDal>();    
            _cacheManager = A.Fake<ICacheManager>();  
            _mapper = A.Fake<IMapper>();
            _stockManager = new StockManager(_stockDal,_cacheManager,_mapper);

        }

        [Test]
        public  async Task Add_StockEntity_AddDatabaseStockEntity()
        {
            var fakestockDto = FakeDataGenerator.FakeDataGenerator.FakeAddStockDto();
           
            await _stockManager.AddAsync(fakestockDto);

            
            A.CallTo(() => _stockDal.AddAsync(A<EntityLayer.Concrete.Stock>.Ignored))
               .MustHaveHappenedOnceExactly();

            A.CallTo(() => _cacheManager.RemoveByPattern("Stock"))
              .MustHaveHappenedOnceExactly();

        }

        [Test]
        public async Task Delete_StockEntity_DeleteStockEntityFromDatabase()
        {
            string id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728";

        
            await _stockManager.DeleteAsync(id);

            A.CallTo(() => _stockDal.RemoveAsync(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.RemoveByPattern("Stock")).MustHaveHappenedOnceExactly();
        }


        [Test]
        public async Task GetAll_StockDtoValues_GetStockListWhenDataIsInCache()
        {
            string cacheKey = "Stock.GetAll";
            var StockDtos = FakeDataGenerator.FakeDataGenerator.FakeStockDtoList();
            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).Returns(true);    
            A.CallTo(() => _cacheManager.Get<List<StockDto>>(cacheKey)).Returns(StockDtos);

            var result = await _stockManager.GetAllAsync();

            Assert.That(result , Is.EqualTo(StockDtos));

            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.Get<List<StockDto>>(cacheKey)).MustHaveHappenedOnceExactly();

        }

        [Test]
        public async Task GetAll_StockDtoValues_GetStockListWhenDataIsNotInCache()
        {
            string cacheKey = "Stock.GetAll";
            var StockDtos = FakeDataGenerator.FakeDataGenerator.FakeStockDtoList();

            var stocks = new List<EntityLayer.Concrete.Stock>()
            {
                 new EntityLayer.Concrete.Stock {Id="d6a682c3-9a3b-4c44-bd95-0e5f846ab728" ,ProductId = "9aa316f4-d166-4519-9cb7-bef83ccc6f76", Count = 10 },
                 new EntityLayer.Concrete.Stock {Id="f37ae358-92c1-492e-98e0-62abb418bf79" ,ProductId = "b0e0c282-4e46-4c39-b499-93953f3acdee", Count = 20 },
                 new EntityLayer.Concrete.Stock {Id="6a95af01-a24b-41a8-bded-c796a9025422" ,ProductId = "8c44800b-b116-45ee-beb3-ec925f0e3e36", Count = 30 },
                 new EntityLayer.Concrete.Stock {Id="d2796d9b-7148-45f9-8602-60ed46ce2e62" ,ProductId = "dee1f1fb-ec89-424b-b04d-1318670d18c1", Count = 40 },
            };

            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).Returns(false);
            A.CallTo(() => _stockDal.GetAllAsync()).Returns(stocks);
            A.CallTo(() => _mapper.Map<List<StockDto>>(stocks)).Returns(StockDtos);

            var result = await _stockManager.GetAllAsync();

            Assert.That(result, Is.EqualTo(StockDtos));

            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _stockDal.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<List<StockDto>>(stocks)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.Add(cacheKey, StockDtos, 60)).MustHaveHappenedOnceExactly();
        }

        [Test]

        public async Task GetById_StockDtoValue_ReturnStockDtoByIdWhenDataIsInCache()
        {
            string id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728";
            var cacheKey = $"Stock.GetById.{id}";
            var stockDto = FakeDataGenerator.FakeDataGenerator.FakeStockDto();

            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).Returns(true);
            A.CallTo(() => _cacheManager.Get<StockDto>(cacheKey)).Returns(stockDto);

            var result = await _stockManager.GetByIdAsync(id);

            Assert.That(result, Is.EqualTo(stockDto));
            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.Get<StockDto>(cacheKey)).MustHaveHappenedOnceExactly();
        }

        [Test]

        public async Task GetById_StockDtoValue_ReturnStockDtoByIdWhenDataIsNotInCache()
        {
            string id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728";
            var cacheKey = $"Stock.GetById.{id}";
            var stockDto = FakeDataGenerator.FakeDataGenerator.FakeStockDto();
            var stock = FakeDataGenerator.FakeDataGenerator.FakeStockEntity();

            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).Returns(false);
            A.CallTo(() => _stockDal.GetByIdAsync(id)).Returns(stock);
            A.CallTo(() => _mapper.Map<StockDto>(stock)).Returns(stockDto);

            var result = await _stockManager.GetByIdAsync(id);

            Assert.That(result , Is.EqualTo(stockDto));


            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _stockDal.GetByIdAsync(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<StockDto>(stock)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.Add(cacheKey, result, 60)).MustHaveHappenedOnceExactly();

        }

        [Test]
        public async Task StockUpdate_StockEntityValue_UpdateStockEntityInDatabase()
        {
            var updateStockDto =FakeDataGenerator.FakeDataGenerator.FakeUpdateStockDto();
            var stock = FakeDataGenerator.FakeDataGenerator.FakeStockEntity();

            A.CallTo(() => _mapper.Map<EntityLayer.Concrete.Stock>(updateStockDto)).Returns(stock);
            await _stockManager.UpdateAsync(updateStockDto);

            // Assert
            A.CallTo(() => _mapper.Map<EntityLayer.Concrete.Stock>(updateStockDto)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _stockDal.UpdateAsync(stock, updateStockDto.Id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.RemoveByPattern("Stock")).MustHaveHappenedOnceExactly();

        }
    }
}
