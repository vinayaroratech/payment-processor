using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Payments.CommandHandlers;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.UnitTests.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandTests : CommandTestBase
    {
        [Test]
        public async Task Handle_ShouldPersistPayment()
        {
            var command = new CreatePaymentCommand
            {
                Name = "Do yet another thing."
            };

            var sut = new CreatePaymentCommandHandler(Context);

            var result = await sut.Handle(command, CancellationToken.None);

            var entity = Context.Payments.Find(result);

            entity.Should().NotBeNull();
            entity.Name.Should().Be(command.Name);
            entity.IsComplete.Should().BeFalse();
        }
    }
}