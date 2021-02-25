using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Mappings;
using Payments.Application.Common.Models;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Application.Payments.Queries.GetPaymentsWithPagination;
using Payments.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Data.Repositories
{
    public class EfPaymentRepository : EfRepository<Payment, IApplicationDbContext>, IPaymentRepository
    {
        private readonly IMapper _mapper;

        public EfPaymentRepository(IApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PaginationResponse<PaymentDto>> GetPaymentsWithPaginationQuery(GetPaymentsWithPaginationQuery request)
        {
            PaginatedList<PaymentDto> list = await Entity
                   .AsNoTracking()
                   .Where(x => x.CardHolder.Contains(request.SearchText))
                   .OrderBy(x => x.CardHolder)
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
