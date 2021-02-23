using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Mappings;
using Payments.Application.Common.Models;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Application.Payments.Queries.GetPaymentsWithPagination;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Queries.GetPaymentsWithPagination
{
    public class GetPaymentsWithPaginationQueryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public GetPaymentsWithPaginationQueryTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _context = ApplicationDbContextFactory.Create();
            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public async Task Handle_ReturnsPaginatedVmAndPaymentsCount()
        {
            var sut = new GetPaymentsWithPaginationQueryHandler(_context, _mapper);

            var result = await sut.Handle(new GetPaymentsWithPaginationQuery() { SearchText = "i", PageSize = 1 }, CancellationToken.None);

            result.Should().BeOfType<PaginationResponse<PaymentDto>>();
            result.PageIndex.Should().Be(1);
            result.TotalPages.Should().Be(4);
            result.TotalCount.Should().Be(4);
            result.Items.Count.Should().Be(1);
        }
    }
}