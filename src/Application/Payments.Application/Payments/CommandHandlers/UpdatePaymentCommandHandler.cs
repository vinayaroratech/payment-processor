using MediatR;
using Payments.Application.Common.Exceptions;
using Payments.Application.Common.Interfaces;
using Payments.Application.Payments.Commands.UpdatePayment;
using Payments.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.CommandHandlers
{
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, long>
    {
        private readonly IPaymentRepository _repository;

        public UpdatePaymentCommandHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<long> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Payment), request.Id);
            }

            entity.CardHolder = request.CardHolder;
            entity.IsComplete = request.IsComplete;

            await _repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

            return entity.Id;
        }
    }
}