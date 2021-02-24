using FluentAssertions;
using NUnit.Framework;
using Payments.Domain.Entities;
using Payments.Infrastructure.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payments.Infrastructure.IntegrationTests.Persistence
{
    public class EfRepositoryTests : BaseTests
    {
        private readonly EfPaymentRepository _paymentRepository;
        public EfRepositoryTests() : base()
        {
            //_paymentDbSetMock = new Mock<DbSet<Payment>>();

            //_paymentDbSetMock.Setup(s => s.FindAsync(It.IsAny<Guid>())).Returns(ValueTask.FromResult(new Payment()));
            //_sut.Setup(s => s.Set<Payment>()).Returns(_paymentDbSetMock.Object);

            _paymentRepository = new EfPaymentRepository(_sut);
        }

        [Test]
        public async Task AddAsync_GivenNewPayment_ShouldSetCreatedProperties()
        {
            var payment = new Payment
            {
                Id = 2,
                Name = "This thing is done.",
                IsComplete = true
            };

            await _paymentRepository.AddAsync(payment);

            payment.Created.Should().Be(_dateTime);
            payment.CreatedBy.Should().Be(_userId);
        }

        [Test]
        public async Task UpdateAsync_GivenExistingPayment_ShouldSetLastModifiedProperties()
        {
            long id = 3;
            var payment = new Payment
            {
                Id = id,
                Name = "This thing is done.",
                IsComplete = false
            };

            await _paymentRepository.AddAsync(payment);

            var existingPayment = await _paymentRepository.GetByIdAsync(id);

            existingPayment.IsComplete = true;

            await _paymentRepository.UpdateAsync(existingPayment);

            existingPayment.LastModified.Should().NotBeNull();
            existingPayment.LastModified.Should().Be(_dateTime);
            existingPayment.LastModifiedBy.Should().Be(_userId);
        }


        [Test]
        public async Task GetByIdAsync_GivenNewGuid_ShouldReturnsPayment()
        {
            long id = 4;
            var payment = new Payment
            {
                Id = id,
                Name = "This thing is done.",
                IsComplete = false
            };

            await _paymentRepository.AddAsync(payment);

            Payment existingPayment = await _paymentRepository.GetByIdAsync(id);

            existingPayment.Created.Should().Be(_dateTime);
            existingPayment.CreatedBy.Should().Be(_userId);
            existingPayment.LastModified.Should().BeNull();
            existingPayment.LastModifiedBy.Should().BeNull();
        }

        public override void Dispose()
        {
            base.Dispose();
            _paymentRepository?.Dispose();
        }
    }
}