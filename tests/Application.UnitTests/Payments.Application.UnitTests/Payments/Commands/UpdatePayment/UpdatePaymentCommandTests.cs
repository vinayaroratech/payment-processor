using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.Payments.Commands.UpdatePayment;
using Payments.Application.UnitTests.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommandTests : CommandTestBase
    {
        [Test]
        public async Task Handle_GivenValidId_ShouldUpdatePersistedPayment()
        {
            var command = new UpdatePaymentCommand
            {
                Id = 1,
                Name = "This thing is also done.",
                IsComplete = true
            };

            var sut = new UpdatePaymentCommandHandler(Context);

            await sut.Handle(command, CancellationToken.None);

            var entity = Context.Payments.Find(command.Id);

            entity.Should().NotBeNull();
            entity.Name.Should().Be(command.Name);
            entity.IsComplete.Should().BeTrue();
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new UpdatePaymentCommand
            {
                Id = 99,
                Name = "This item doesn't exist.",
                IsComplete = false
            };

            var sut = new UpdatePaymentCommandHandler(Context);

            Assert.ThrowsAsync<NotFoundException>(() =>
                sut.Handle(command, CancellationToken.None));
        }
    }
}