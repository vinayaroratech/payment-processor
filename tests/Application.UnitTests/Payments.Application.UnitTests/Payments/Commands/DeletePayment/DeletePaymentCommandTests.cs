using Payments.Application.Common.Exceptions;
using Payments.Application.Payments.Commands.DeletePayment;
using Payments.Application.UnitTests.Common;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Payments.Application.UnitTests.Payments.Commands.DeletePayment
{
    public class DeletePaymentCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_GivenValidId_ShouldRemovePersistedPayment()
        {
            var command = new DeletePaymentCommand
            {
                Id = 1
            };

            var sut = new DeletePaymentCommandHandler(Context);

            await sut.Handle(command, CancellationToken.None);

            var entity = Context.Payments.Find(command.Id);

            entity.ShouldBeNull();
        }

        [Fact]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new DeletePaymentCommand
            {
                Id = 99
            };

            var sut = new DeletePaymentCommandHandler(Context);

            Should.ThrowAsync<NotFoundException>(() =>
                sut.Handle(command, CancellationToken.None));
        }
    }
}