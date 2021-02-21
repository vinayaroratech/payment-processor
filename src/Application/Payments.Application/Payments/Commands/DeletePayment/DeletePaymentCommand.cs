using MediatR;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Interfaces.Contexts;
using Payments.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Commands.DeletePayment
{
    public class DeletePaymentCommand : IRequest<long>
    {
        public long Id { get; set; }

        public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, long>
        {
            private readonly IApplicationDbContext _context;

            public DeletePaymentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Payments.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Payment), request.Id);
                }

                _context.Payments.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
