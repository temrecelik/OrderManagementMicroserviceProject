using CompanyService.DataAccessLayer.Context;
using CompanyService.WebApi.Consumer;
using FakeItEasy;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Test.CompanyService.WebApi.Consumer
{
    [TestFixture]
    public class OrderCreatedEventConsumerTest
    {
        private OrderCreatedEventConsumer _orderCreatedEventConsumer;
        private ISendEndpointProvider _sendEndpointProvider;
        private IPublishEndpoint _publishEndpoint;
        private CompanyDbContext _CompanyDbContext;
        private DbContextOptions<CompanyDbContext> _dbContextOptions;

        [SetUp]
        public void SetUp()
        {
            _sendEndpointProvider = A.Fake<ISendEndpointProvider>();
            _publishEndpoint = A.Fake<IPublishEndpoint>();

            _dbContextOptions = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            _CompanyDbContext = new CompanyDbContext(_dbContextOptions);

            _orderCreatedEventConsumer = new OrderCreatedEventConsumer(_sendEndpointProvider, _publishEndpoint, _CompanyDbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _CompanyDbContext.Database.EnsureDeleted();
            _CompanyDbContext.Dispose();
        }

        [Test]
        public async Task Consume_ShouldPublishCompanyWorkingHoursSuitableEvent_WhenCompanyIsOpen()
        {

        }
    }


   
}
