using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Services.PaymentGateway;
using Payments.Infrastructure.Data.Repositories;
using Payments.Infrastructure.IntegrationTests.Persistence;
using Payments.Infrastructure.Services.PaymentGateway;
using System.Threading.Tasks;

namespace Payments.Infrastructure.IntegrationTests.Services
{
    class PaymentStrategyTests : BaseTests
    {
        private readonly IPaymentStrategy _paymentStrategy;
        public PaymentStrategyTests() : base()
        {
            var paymentRepository = new EfPaymentRepository(_sut, Mapper);
            _paymentStrategy = new PaymentStrategy(
                    new IPaymentService[] {
                        new CheapPaymentService(paymentRepository),
                        new ExpensivePaymentService(paymentRepository),
                        new PremiumPaymentService(paymentRepository)
            });
        }

        [Test]
        public void GetPaymentService_GivenNewPayment_ShouldReturnCheapPaymentService()
        {
            var payment = new CreditCardModel
            {
                Amount = 19.99M
            };

            IPaymentService cheapPaymentService = _paymentStrategy.GetPaymentService(payment);

            cheapPaymentService.Should().NotBeNull();
            cheapPaymentService.Should().BeOfType(typeof(CheapPaymentService));
        }


        [Test]
        public void GetPaymentService_GivenNewPayment_ShouldReturnExpensivePaymentService()
        {
            var payment = new CreditCardModel
            {
                Amount = 499.999M
            };

            IPaymentService cheapPaymentService = _paymentStrategy.GetPaymentService(payment);

            cheapPaymentService.Should().NotBeNull();
            cheapPaymentService.Should().BeOfType(typeof(ExpensivePaymentService));
        }


        [Test]
        public void GetPaymentService_GivenNewPayment_ShouldReturnPremiumPaymentService()
        {
            var payment = new CreditCardModel
            {
                Amount = 500.1M
            };

            IPaymentService cheapPaymentService = _paymentStrategy.GetPaymentService(payment);

            cheapPaymentService.Should().NotBeNull();
            cheapPaymentService.Should().BeOfType(typeof(PremiumPaymentService));
        }

        [Test]
        public async Task MakePayment_GivenNewPayment_ShouldMakePayment()
        {
            var payment = new CreditCardModel
            {
                Amount = 500.1M
            };

            var paymentId = await _paymentStrategy.MakePaymentAsync(payment);
            paymentId.Should().BeGreaterThan(0);
        }
    }
}