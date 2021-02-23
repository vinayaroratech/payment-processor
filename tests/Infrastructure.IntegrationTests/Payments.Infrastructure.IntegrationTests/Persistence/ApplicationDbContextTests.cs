using FluentAssertions;
using IdentityServer4.EntityFramework.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Common;
using Payments.Domain.Entities;
using Payments.Infrastructure.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Infrastructure.IntegrationTests.Persistence
{
    public class ApplicationDbContextTests : IDisposable
    {
        private readonly string _userId;
        private readonly DateTime _dateTime;
        private readonly Mock<IDateTime> _dateTimeMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly ApplicationDbContext _sut;
        private readonly Mock<IDomainEventService> _domainEventServiceMock;

        public ApplicationDbContextTests()
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

        [Test]
        public async Task SaveChangesAsync_GivenNewProduct_ShouldSetCreatedProperties()
        {
            var payment = new Payment
            {
                Id = 2,
                Name = "This thing is done.",
                IsComplete = true
            };

            _sut.Payments.Add(payment);

            await _sut.SaveChangesAsync();

            payment.Created.Should().Be(_dateTime);
            payment.CreatedBy.Should().Be(_userId);
        }

        [Test]
        public async Task SaveChangesAsync_GivenExistingProduct_ShouldSetLastModifiedProperties()
        {
            long id = 1;

            var product = await _sut.Payments.FindAsync(id);

            product.IsComplete = true;

            await _sut.SaveChangesAsync();

            product.LastModified.Should().NotBeNull();
            product.LastModified.Should().Be(_dateTime);
            product.LastModifiedBy.Should().Be(_userId);
        }

        public void Dispose()
        {
            _sut?.Dispose();
        }
    }
}