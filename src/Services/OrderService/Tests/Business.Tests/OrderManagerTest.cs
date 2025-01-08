using AutoFixture;
using AutoMapper;
using FakeItEasy;
using Order.BusinessLayer.Concrete;
using Order.DataAccessLayer.Abstract;
using Order.EntityLayer.Dtos;
using Shared.Core.CrossCuttingConcerns.Caching;

namespace Business.Tests;

[TestFixture]
public class OrderManagerTest
{
    private IOrderDal _fakeOrderDal;
    private IMapper _fakeMapper;
    private ICacheManager _fakeCacheManager;
    private OrderManager _orderManager;
    private IFixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fakeOrderDal = A.Fake<IOrderDal>();
        _fakeMapper = A.Fake<IMapper>();
        _fakeCacheManager = A.Fake<ICacheManager>();

        _orderManager = new OrderManager(_fakeOrderDal, _fakeMapper, _fakeCacheManager);

        _fixture = new Fixture();
    }

    //[MethodName]_[Condition]_[ExpectedResult] => Test isimlendirme biçimi

    [Test]
    public void AddAsync_WhenCalled_AddsOrderAndRemovesCache()
    {
        var createOrderDto = _fixture.Create<CreateOrderDto>();
        var orderEntity = _fixture.Create<Order.EntityLayer.Concrete.Order>();

        A.CallTo(() => _fakeMapper.Map<Order.EntityLayer.Concrete.Order>(createOrderDto)).Returns(orderEntity);
        A.CallTo(() => _fakeOrderDal.AddAsync(orderEntity)).Returns(Task.FromResult(true));

        var result = _orderManager.AddAsync(createOrderDto);

        Assert.AreEqual(orderEntity, result.Result);
        A.CallTo(() => _fakeOrderDal.AddAsync(A<Order.EntityLayer.Concrete.Order>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeOrderDal.SaveAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeCacheManager.RemoveByPattern(Constants.AddTestCacheKey)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void GetAll_WhenCacheIsNotAvailable_ShouldGetDataFromDatabaseAndAddToCache()
    {
        // AutoFixture ile sahte veri üretme
        var orderEntity = _fixture.Create<List<Order.EntityLayer.Concrete.Order>>();
        var getOrderDto = _fixture.Create<List<GetOrderDto>>();

        A.CallTo(() => _fakeCacheManager.IsAdd(Constants.GetAllTestCacheKey)).Returns(false);// Cache'de veri olmadýðýný taklit et
        A.CallTo(() => _fakeOrderDal.GetAll(null)).Returns(orderEntity.AsQueryable());// Veritabanýndan veri dönecek olan taklit
        A.CallTo(() => _fakeMapper.Map<List<GetOrderDto>>(orderEntity)).Returns(getOrderDto);// Mapper'ýn dönüþünü taklit et

        var result = _orderManager.GetAll();

        Assert.AreEqual(getOrderDto, result);
        // Veritabanýna gidildiðini ve verilerin cache'e eklendiðini doðrula
        A.CallTo(() => _fakeOrderDal.GetAll(null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeCacheManager.Add(Constants.GetAllTestCacheKey, getOrderDto, 60)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void GetAll_WhenCacheIsAvailable_ShouldReturnDataFromCache()
    {
        var cachedOrders = _fixture.Create<List<GetOrderDto>>();// AutoFixture ile sahte veri üretme

        A.CallTo(() => _fakeCacheManager.IsAdd(Constants.GetAllTestCacheKey)).Returns(true);
        A.CallTo(() => _fakeCacheManager.Get<List<GetOrderDto>>(Constants.GetAllTestCacheKey)).Returns(cachedOrders);

        var result = _orderManager.GetAll();

        Assert.AreEqual(cachedOrders, result);
        A.CallTo(() => _fakeOrderDal.GetAll(null)).MustNotHaveHappened(); //Veritabanýna gitmemeli
    }

    [Test]
    public void GetByIdAsync_WhenCacheIsNotAvailable_ShouldGetDataFromDatabaseAndAddToCache()
    {
        var orderEntity = _fixture.Create<Order.EntityLayer.Concrete.Order>();
        var getOrderDto = _fixture.Create<GetOrderDto>();

        A.CallTo(() => _fakeCacheManager.IsAdd(Constants.GetByIdTestCacheKey)).Returns(false);
        A.CallTo(() => _fakeOrderDal.GetByIdAsync(Constants.GetByIdTestId)).Returns(Task.FromResult(orderEntity));
        A.CallTo(() => _fakeMapper.Map<GetOrderDto>(orderEntity)).Returns(getOrderDto);

        var result = _orderManager.GetByIdAsync(Constants.GetByIdTestId);

        Assert.AreEqual(getOrderDto, result.Result);
        A.CallTo(() => _fakeOrderDal.GetByIdAsync(Constants.GetByIdTestId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeCacheManager.Add(Constants.GetByIdTestCacheKey, getOrderDto, 60)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void GetByIdAsync_WhenCacheIsAvailable_ShouldReturnDataFromCache()
    {
        var cacheOrder = _fixture.Create<GetOrderDto>();

        A.CallTo(() => _fakeCacheManager.IsAdd(Constants.GetByIdTestCacheKey)).Returns(true);
        A.CallTo(() => _fakeCacheManager.Get<GetOrderDto>(Constants.GetByIdTestCacheKey)).Returns(cacheOrder);

        var result = _orderManager.GetByIdAsync(Constants.GetByIdTestId);

        Assert.AreEqual(cacheOrder, result.Result);
        A.CallTo(() => _fakeOrderDal.GetByIdAsync(Constants.GetByIdTestId)).MustNotHaveHappened();
    }
}