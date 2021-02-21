using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Queries.GetPaymentsList
{
    public class GetPaymentsListQuery : IRequest<PaymentsListVm>
    {
        public long Id { get; set; }

        public class GetPaymentsListQueryHandler : IRequestHandler<GetPaymentsListQuery, PaymentsListVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetPaymentsListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PaymentsListVm> Handle(GetPaymentsListQuery request, CancellationToken cancellationToken)
            {
                var items = await _context.Payments
                    .ProjectTo<PaymentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var vm = new PaymentsListVm
                {
                    Payments = items
                };

                return vm;
            }
        }
    }
}