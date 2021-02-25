using FluentAssertions;
using Moq;
using NUnit.Framework;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.CommandHandlers;
using Payments.Application.Payments.Commands.ProcessPayment;
using Payments.Application.Services.PaymentGateway;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Commands
{
    public class ProcessPaymentCommandTests : CommandTestBase
    {
        private readonly IPaymentRepository _repository;
        private readonly Mock<IPaymentStrategy> _paymentStrategyMock;
        private readonly Mock<IPaymentService> _paymentServiceMock;

        public ProcessPaymentCommandTests() : base()
        {
            _repository = new EfPaymentRepository(Context, Mapper);
            _paymentStrategyMock = new Mock<IPaymentStrategy>();
            _paymentServiceMock = new Mock<IPaymentService>();

            _paymentStrategyMock.Setup(m => m.MakePaymentAsync(It.IsAny<IPaymentModel>()))
                .Returns(Task.FromResult<long>(1));
            _paymentStrategyMock.Setup(m => m.GetPaymentService(It.IsAny<IPaymentModel>()))
                .Returns(_paymentServiceMock.Object);
        }

        [Test]
        public async Task Handle_ShouldPersistPayment()
        {
            var command = new ProcessPaymentCommand
            {
                CardHolder = "Do yet another thing."
            };

            var sut = new ProcessPaymentCommandHandler(_paymentStrategyMock.Object);

            var result = await sut.Handle(command, CancellationToken.None);

            var entity = await _repository.GetByIdAsync(result);

            entity.Should().NotBeNull();
            entity.CardHolder.Should().Be("Do this thing.");
            entity.IsComplete.Should().BeFalse();
        }
    }
}