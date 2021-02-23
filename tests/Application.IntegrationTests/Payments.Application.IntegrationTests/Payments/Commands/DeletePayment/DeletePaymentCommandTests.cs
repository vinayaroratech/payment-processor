using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Commands.DeletePayment;
using Payments.Domain.Entities;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.Payments.Commands.DeletePayment
{
    using static Testing;
    public class DeletePaymentCommandTests : TestBase
    {
        [Test]
        public async Task ShouldDeletePayment()
        {
            long id = 1;

            await SendAsync(new DeletePaymentCommand { Id = id });

            var payment = await FindAsync<Payment>(id);

            payment.Should().BeNull();
        }

        [Test]
        public void ShouldRequireValidPaymentId()
        {
            var command = new DeletePaymentCommand { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();

        }
    }
}