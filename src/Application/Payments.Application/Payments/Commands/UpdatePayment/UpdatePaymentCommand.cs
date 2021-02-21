using MediatR;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Interfaces.Contexts;
using Payments.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : IRequest<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, long>
        {
            private readonly IApplicationDbContext _context;

            public UpdatePaymentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Payments.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Payment), request.Id);
                }

                entity.Name = request.Name;
                entity.IsComplete = request.IsComplete;

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
