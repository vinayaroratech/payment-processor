using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Queries.GetPayment
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, PaymentVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaymentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaymentVm> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Payments
                .AsNoTracking()
                .Where(t => t.Id == request.Id)
                .ProjectTo<PaymentVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (vm == null)
            {
                throw new NotFoundException(nameof(Payment), request.Id);
            }

            return vm;
        }
    }
}