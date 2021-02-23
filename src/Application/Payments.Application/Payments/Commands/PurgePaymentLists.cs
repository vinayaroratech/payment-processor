using MediatR;
using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Commands
{
    [Authorize(Roles = "Administrator")]
    [Authorize(Policy = "CanPurge")]
    public class PurgePaymentListsCommand : IRequest
    {
    }

    public class PurgePaymentListsCommandHandler : IRequestHandler<PurgePaymentListsCommand>
    {
        private readonly IApplicationDbContext _context;

        public PurgePaymentListsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PurgePaymentListsCommand request, CancellationToken cancellationToken)
        {
            _context.Payments.RemoveRange(_context.Payments);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}