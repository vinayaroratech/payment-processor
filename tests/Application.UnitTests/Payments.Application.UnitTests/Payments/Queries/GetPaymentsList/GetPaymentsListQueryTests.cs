using AutoMapper;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Persistence;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Payments.Application.UnitTests.Payments.Queries.GetPaymentsList
{
    [Collection("QueryCollection")]
    public class GetPaymentsListQueryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaymentsListQueryTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_ReturnsCorrectVmAndTPaymentsCount()
        {
            var sut = new GetPaymentsListQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPaymentsListQuery(), CancellationToken.None);

            result.ShouldBeOfType<PaymentsListVm>();
            result.Payments.Count.ShouldBe(4);
        }
    }
}