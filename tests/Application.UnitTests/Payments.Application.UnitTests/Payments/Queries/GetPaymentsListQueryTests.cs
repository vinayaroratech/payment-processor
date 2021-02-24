using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Mappings;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Queries
{
    public class GetPaymentsListQueryTests : CommandTestBase
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentsListQueryTests() : base()
        {
            _repository = new EfPaymentRepository(Context, Mapper);
        }

        [Test]
        public async Task Handle_ReturnsCorrectVmAndPaymentsCount()
        {
            var sut = new GetPaymentsListQueryHandler(_repository, Mapper);

            var result = await sut.Handle(new GetPaymentsListQuery(), CancellationToken.None).ConfigureAwait(false);

            result.Should().BeOfType<PaymentsListVm>();
            result.Payments.Count.Should().Be(4);
        }
    }
}