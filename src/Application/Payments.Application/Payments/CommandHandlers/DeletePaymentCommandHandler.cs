using MediatR;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.Commands.DeletePayment;
using Payments.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.CommandHandlers
{
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, long>
    {
        private readonly IPaymentRepository _repository;

        public DeletePaymentCommandHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<long> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Payment), request.Id);
            }

            await _repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

            return entity.Id;
        }
    }
}