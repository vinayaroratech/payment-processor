using AutoMapper;
using MediatR;
using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Models;
using Payments.Application.Payments.Queries.GetPaymentsList;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Queries.GetPaymentsWithPagination
{
    public class GetPaymentsWithPaginationQueryHandler : IRequestHandler<GetPaymentsWithPaginationQuery, PaginationResponse<PaymentDto>>
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;

        public GetPaymentsWithPaginationQueryHandler(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<PaginationResponse<PaymentDto>> Handle(GetPaymentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetPaymentsWithPaginationQuery(request);
        }
    }
}