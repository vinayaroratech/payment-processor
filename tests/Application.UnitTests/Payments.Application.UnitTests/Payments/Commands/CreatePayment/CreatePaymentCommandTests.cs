using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.UnitTests.Common;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Payments.Application.UnitTests.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_ShouldPersistPayment()
        {
            var command = new CreatePaymentCommand
            {
                Name = "Do yet another thing."
            };

            var sut = new CreatePaymentCommandHandler(Context);

            var result = await sut.Handle(command, CancellationToken.None);

            var entity = Context.Payments.Find(result);

            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(command.Name);
            entity.IsComplete.ShouldBeFalse();
        }
    }
}