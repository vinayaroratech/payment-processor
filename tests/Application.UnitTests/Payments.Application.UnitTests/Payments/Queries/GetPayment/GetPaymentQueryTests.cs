using AutoMapper;
using Payments.Application.Common.Exceptions;
using Payments.Application.Payments.Queries.GetPayment;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Persistence;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Payments.Application.UnitTests.Payments.Queries.GetPayment
{
    [Collection("QueryCollection")]
    public class GetPaymentQueryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaymentQueryTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Handle_GivenValidId_ReturnsCorrectVm()
        {
            var query = new GetPaymentQuery
            {
                Id = 1
            };

            var sut = new GetPaymentQueryHandler(_context, _mapper);

            var result = await sut.Handle(query, CancellationToken.None);

            result.ShouldBeOfType<PaymentVm>();
            result.Id.ShouldBe(1);
        }

        [Fact]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var query = new GetPaymentQuery
            {
                Id = 99
            };

            var sut = new GetPaymentQueryHandler(_context, _mapper);

            Should.ThrowAsync<NotFoundException>(() =>
                sut.Handle(query, CancellationToken.None));
        }
    }
}