using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Mappings;
using Payments.Application.Payments.Queries.GetPayment;
using Payments.Application.UnitTests.Common;
using Payments.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.UnitTests.Payments.Queries.GetPayment
{
    public class GetPaymentQueryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public GetPaymentQueryTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _context = ApplicationDbContextFactory.Create();
            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public async Task Handle_GivenValidId_ReturnsCorrectVm()
        {
            var query = new GetPaymentQuery
            {
                Id = 1
            };

            var sut = new GetPaymentQueryHandler(_context, _mapper);

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

            var sut = new GetPaymentQueryHandler(_context, _mapper);

            Assert.ThrowsAsync<NotFoundException>(() =>
                sut.Handle(query, CancellationToken.None));
        }
    }
}