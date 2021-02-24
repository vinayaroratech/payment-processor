using FluentAssertions;
using NUnit.Framework;
using Payments.Domain.Entities;
using System.Threading.Tasks;

namespace Payments.Infrastructure.IntegrationTests.Persistence
{
    public class ApplicationDbContextTests : BaseTests
    {
        public ApplicationDbContextTests() : base()
        {
        }

        [Test]
        public async Task SaveChangesAsync_GivenNewPayment_ShouldSetCreatedProperties()
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
        public async Task SaveChangesAsync_GivenExistingPayment_ShouldSetLastModifiedProperties()
        {
            long id = 1;

            var payment = await _sut.Payments.FindAsync(id);

            payment.IsComplete = true;

            await _sut.SaveChangesAsync();

            payment.LastModified.Should().NotBeNull();
            payment.LastModified.Should().Be(_dateTime);
            payment.LastModifiedBy.Should().Be(_userId);
        }
    }
}