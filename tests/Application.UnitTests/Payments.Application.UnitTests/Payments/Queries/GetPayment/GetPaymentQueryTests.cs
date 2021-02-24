using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.Queries.GetPayment;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Queries.GetPayment
{
    public class GetPaymentQueryTests : CommandTestBase
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentQueryTests() : base()
        {
            _repository = new EfPaymentRepository(Context, Mapper);
        }

        [Test]
        public async Task Handle_GivenValidId_ReturnsCorrectVm()
        {
            var query = new GetPaymentQuery
            {
                Id = 1
            };

            var sut = new GetPaymentQueryHandler(_repository, Mapper);

            var result = await sut.Handle(query, CancellationToken.None);

            result.Should().BeOfType<PaymentVm>();
            result.Id.Should().Be(1);
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var query = new GetPaymentQuery
            {
                Id = 99
            };

            var sut = new GetPaymentQueryHandler(_repository, Mapper);

            Assert.ThrowsAsync<NotFoundException>(() =>
                sut.Handle(query, CancellationToken.None));
        }
    }
}