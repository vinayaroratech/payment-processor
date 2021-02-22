using Payments.Application.Common.Exceptions;
using Payments.Application.Payments.Commands.UpdatePayment;
using Payments.Application.UnitTests.Common;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Payments.Application.UnitTests.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommandTests : CommandTestBase
    {
        [Fact]
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

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(command.Name);
            entity.IsComplete.ShouldBeTrue();
        }

        [Fact]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new UpdatePaymentCommand
            {
                Id = 99,
                Name = "This item doesn't exist.",
                IsComplete = false
            };

            var sut = new UpdatePaymentCommandHandler(Context);

            Should.ThrowAsync<NotFoundException>(() =>
                sut.Handle(command, CancellationToken.None));
        }
    }
}