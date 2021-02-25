using Payments.Application.Common.Interfaces;
using Payments.Application.Services.PaymentGateway;
using Payments.Domain.Entities;
using Payments.Domain.Events;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Services.PaymentGateway
{
    public class ExpensivePaymentService : PaymentService<CreditCardModel>
    {
        private readonly IPaymentRepository _repository;

        public ExpensivePaymentService(IPaymentRepository repository)
        {
            _repository = repository;
        }

        protected override bool AppliesTo(CreditCardModel model)
        {
            return model.Amount >= 21 && model.Amount <= 500;
        }

        protected override async Task<long> MakePaymentAsync(CreditCardModel model)
        {
            var entity = new Payment
            {
                CardHolder = model.CardHolder,
                Amount = model.Amount,
                CreditCardNumber = model.CreditCardNumber,
                ExpirationDate = model.ExpirationDate,
                SecurityCode = model.SecurityCode,
                IsComplete = false
            };

            entity.DomainEvents.Add(new PaymentCreatedEvent(entity));
            await _repository.AddAsync(entity);

            return entity.Id;
        }
    }
}
