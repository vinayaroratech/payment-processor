using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Common;
using Payments.Domain.Entities;
using Payments.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace Payments.Infrastructure.IntegrationTests.Persistence
{
    public abstract class BaseTests : IDisposable
    {
        protected readonly string _userId;
        protected readonly DateTime _dateTime;
        protected readonly Mock<IDateTime> _dateTimeMock;
        protected readonly Mock<ICurrentUserService> _currentUserServiceMock;
        protected readonly ApplicationDbContext _sut;
        protected readonly Mock<IDomainEventService> _domainEventServiceMock;
        
        protected BaseTests()
        {
            _dateTime = new DateTime(3001, 1, 1);
            _dateTimeMock = new Mock<IDateTime>();
            _dateTimeMock.Setup(m => m.Now).Returns(_dateTime);

            _domainEventServiceMock = new Mock<IDomainEventService>();
            _domainEventServiceMock.Setup(m => m.Publish(It.IsAny<DomainEvent>()))
                .Returns(Task.CompletedTask);

            _userId = "00000000-0000-0000-0000-000000000000";
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock.Setup(m => m.UserId).Returns(_userId);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var operationalStoreOptions = Options.Create(
                new OperationalStoreOptions
                {
                    DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
                    PersistedGrants = new TableConfiguration("PersistedGrants")
                });

            _sut = new ApplicationDbContext(options, operationalStoreOptions, _currentUserServiceMock.Object, _dateTimeMock.Object, _domainEventServiceMock.Object);

            _sut.Payments.Add(new Payment
            {
                Id = 1,
                Name = "Do this thing."
            });

            _sut.SaveChanges();

        }

        public virtual void Dispose()
        {
            _sut?.Dispose();
        }
    }
}