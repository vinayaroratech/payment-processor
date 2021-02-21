using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Entities;
using Payments.Infrastructure.Persistence;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Payments.Infrastructure.IntegrationTests.Persistence
{
    public class ApplicationDbContextTests : IDisposable
    {
        private readonly string _userId;
        private readonly DateTime _dateTime;
        private readonly Mock<IDateTime> _dateTimeMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly ApplicationDbContext _sut;

        public ApplicationDbContextTests()
        {
            _dateTime = new DateTime(3001, 1, 1);
            _dateTimeMock = new Mock<IDateTime>();
            _dateTimeMock.Setup(m => m.Now).Returns(_dateTime);

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

            _sut = new ApplicationDbContext(options, operationalStoreOptions, _currentUserServiceMock.Object, _dateTimeMock.Object);

            _sut.Payments.Add(new Payment
            {
                Id = 1,
                Name = "Do this thing."
            });

            _sut.SaveChanges();
        }

        [Fact]
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

            payment.Created.ShouldBe(_dateTime);
            payment.CreatedBy.ShouldBe(_userId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenExistingProduct_ShouldSetLastModifiedProperties()
        {
            long id = 1;

            var product = await _sut.Payments.FindAsync(id);

            product.IsComplete = true;

            await _sut.SaveChangesAsync();

            product.LastModified.ShouldNotBeNull();
            product.LastModified.ShouldBe(_dateTime);
            product.LastModifiedBy.ShouldBe(_userId);
        }

        public void Dispose()
        {
            _sut?.Dispose();
        }
    }
}