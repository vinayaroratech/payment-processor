using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.CommandHandlers;
using Payments.Application.Payments.Commands.UpdatePayment;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Commands
{
    public class UpdatePaymentCommandTests : CommandTestBase
    {
        private readonly IPaymentRepository _repository;

        public UpdatePaymentCommandTests() : base()
        {
            _repository = new EfPaymentRepository(Context, Mapper);
        }

        [Test]
        public async Task Handle_GivenValidId_ShouldUpdatePersistedPayment()
        {
            var command = new UpdatePaymentCommand
            {
                Id = 1,
                CardHolder = "This thing is also done.",
                IsComplete = true
            };

            var sut = new UpdatePaymentCommandHandler(_repository);

            await sut.Handle(command, CancellationToken.None);

            var entity = await _repository.GetByIdAsync(command.Id).ConfigureAwait(false);

            entity.Should().NotBeNull();
            entity.CardHolder.Should().Be(command.CardHolder);
            entity.IsComplete.Should().BeTrue();
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new UpdatePaymentCommand
            {
                Id = 99,
                CardHolder = "This item doesn't exist.",
                IsComplete = false
            };

            var sut = new UpdatePaymentCommandHandler(_repository);

            Assert.ThrowsAsync<NotFoundException>(() =>
                sut.Handle(command, CancellationToken.None));
        }
    }
}