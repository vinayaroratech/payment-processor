using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Models;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Application.Payments.Queries.GetPaymentsWithPagination;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Queries
{
    public class GetPaymentsWithPaginationQueryTests : CommandTestBase
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentsWithPaginationQueryTests() : base()
        {
            _repository = new EfPaymentRepository(Context, Mapper);
        }


        [Test]
        public async Task Handle_ReturnsPaginatedVmAndPaymentsCount()
        {
            var sut = new GetPaymentsWithPaginationQueryHandler(_repository, Mapper);

            var result = await sut.Handle(new GetPaymentsWithPaginationQuery() { SearchText = "i", PageSize = 1 }, CancellationToken.None);

            result.Should().BeOfType<PaginationResponse<PaymentDto>>();
            result.PageIndex.Should().Be(1);
            result.TotalPages.Should().Be(4);
            result.TotalCount.Should().Be(4);
            result.Items.Count.Should().Be(1);
        }
    }
}