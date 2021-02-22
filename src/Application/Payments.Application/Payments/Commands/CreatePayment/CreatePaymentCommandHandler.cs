using Payments.Application.Common.Interfaces;
using Payments.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, long>
    {
        private readonly IApplicationDbContext _context;

        public CreatePaymentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<long> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Payment
            {
                Name = request.Name,
                IsComplete = false
            };

            _context.Payments.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}