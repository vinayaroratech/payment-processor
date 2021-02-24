using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.CommandHandlers;
using Payments.Application.Payments.Commands.DeletePayment;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Commands.DeletePayment
{
    public class DeletePaymentCommandTests : CommandTestBase
    {
        private readonly IPaymentRepository _repository;

        public DeletePaymentCommandTests() : base()
        {
            _repository = new EfPaymentRepository(Context, Mapper);
        }

        [Test]
        public async Task Handle_GivenValidId_ShouldRemovePersistedPayment()
        {
            var command = new DeletePaymentCommand
            {
                Id = 1
            };

            var sut = new DeletePaymentCommandHandler(_repository);

            await sut.Handle(command, CancellationToken.None);

            var entity = await _repository.GetByIdAsync(command.Id).ConfigureAwait(false);

            entity.Should().BeNull();
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var command = new DeletePaymentCommand
            {
                Id = 99
            };

            var sut = new DeletePaymentCommandHandler(_repository);

            Assert.ThrowsAsync<NotFoundException>(() =>
                sut.Handle(command, CancellationToken.None));
        }
    }
}