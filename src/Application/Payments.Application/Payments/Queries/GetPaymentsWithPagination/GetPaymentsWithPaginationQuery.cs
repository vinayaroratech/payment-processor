using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Mappings;
using Payments.Application.Common.Models;
using Payments.Application.Payments.Queries.GetPaymentsList;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Queries.GetPaymentsWithPagination
{
    public class GetPaymentsWithPaginationQuery : PaginationQuery, IRequest<PaginationResponse<PaymentDto>>
    {
        public string SearchText { get; set; }
    }

    public class GetPaymentsWithPaginationQueryHandler : IRequestHandler<GetPaymentsWithPaginationQuery, PaginationResponse<PaymentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaymentsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<PaymentDto>> Handle(GetPaymentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            PaginatedList<PaymentDto> list = await _context.Payments
                .AsNoTracking()
                .Where(x => x.Name.Contains(request.SearchText))
                .OrderBy(x => x.Name)
                .ProjectTo<PaymentDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);


            return new PaginationResponse<PaymentDto>
            {
                Items = list,
                PageIndex = list.PageIndex,
                TotalPages = list.TotalPages,
                TotalCount = list.TotalCount,
            };
        }
    }
}