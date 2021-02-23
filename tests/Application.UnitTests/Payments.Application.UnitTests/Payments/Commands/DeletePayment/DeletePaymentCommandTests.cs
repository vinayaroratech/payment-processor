using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.Payments.CommandHandlers;
using Payments.Application.Payments.Commands.DeletePayment;
using Payments.Application.UnitTests.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Commands.DeletePayment
{
    public class DeletePaymentCommandTests : CommandTestBase
    {
        [Test]
        public async Task Handle_GivenValidId_ShouldRemovePersistedPayment()
        {
            var command = new DeletePaymentCommand
            {
                Id = 1
            };

            var sut = new DeletePaymentCommandHandler(Context);

            await sut.Handle(command, CancellationToken.None);

            var entity = Context.Payments.Find(command.Id);

            entity.Should().BeNull();
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new DeletePaymentCommand
            {
                Id = 99
            };

            var sut = new DeletePaymentCommandHandler(Context);

            Assert.ThrowsAsync<NotFoundException>(() =>
                sut.Handle(command, CancellationToken.None));
        }
    }
}