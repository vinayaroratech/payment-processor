using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.CommandHandlers;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Commands
{
    public class CreatePaymentCommandTests : CommandTestBase
    {
        private readonly IPaymentRepository _repository;

        public CreatePaymentCommandTests() : base()
        {
            _repository = new EfPaymentRepository(Context, Mapper);
        }

        [Test]
        public async Task Handle_ShouldPersistPayment()
        {
            var command = new CreatePaymentCommand
            {
                CardHolder = "Do yet another thing."
            };

            var sut = new CreatePaymentCommandHandler(_repository);

            var result = await sut.Handle(command, CancellationToken.None);

            var entity = await _repository.GetByIdAsync(result);

            entity.Should().NotBeNull();
            entity.CardHolder.Should().Be(command.CardHolder);
            entity.IsComplete.Should().BeFalse();
        }
    }
}