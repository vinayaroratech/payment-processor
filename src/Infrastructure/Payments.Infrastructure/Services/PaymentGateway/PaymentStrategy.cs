using Payments.Application.Services.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Services.PaymentGateway
{
    public class PaymentStrategy : IPaymentStrategy
    {
        private readonly IEnumerable<IPaymentService> paymentServices;
        public PaymentStrategy(IEnumerable<IPaymentService> paymentServices)
        {
            this.paymentServices = paymentServices ??
                throw new ArgumentNullException(nameof(paymentServices));
        }

        public Task<long> MakePaymentAsync<TPaymentModel>(TPaymentModel model)
            where TPaymentModel : IPaymentModel
        {
           return GetPaymentService(model).MakePaymentAsync(model);
        }

        public IPaymentService GetPaymentService<TPaymentModel>(TPaymentModel model)
            where TPaymentModel : IPaymentModel
        {
            var result = paymentServices.FirstOrDefault(p => p.AppliesTo(model));
            if (result == null)
            {
                throw new InvalidOperationException(
                    $"Payment service for {model.GetType()} not registered.");
            }
            return result;
        }
    }
}
