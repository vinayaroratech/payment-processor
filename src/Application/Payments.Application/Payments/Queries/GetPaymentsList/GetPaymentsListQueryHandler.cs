using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Queries.GetPaymentsList
{
    public class GetPaymentsListQueryHandler : IRequestHandler<GetPaymentsListQuery, PaymentsListVm>
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;

        public GetPaymentsListQueryHandler(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentsListVm> Handle(GetPaymentsListQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.Entity
                .AsNoTracking()
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