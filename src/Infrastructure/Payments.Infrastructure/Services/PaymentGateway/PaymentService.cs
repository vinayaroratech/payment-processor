using Payments.Application.Services.PaymentGateway;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Services.PaymentGateway
{
    public abstract class PaymentService<TModel> : IPaymentService
        where TModel : IPaymentModel
    {
        public virtual bool AppliesTo<T>(T model) where T : IPaymentModel
        {
            return AppliesTo((TModel)(object)model);
        }

        public Task<long> MakePaymentAsync<T>(T model) where T : IPaymentModel
        {
           return MakePaymentAsync((TModel)(object)model);
        }

        protected abstract Task<long> MakePaymentAsync(TModel model);
        protected abstract bool AppliesTo(TModel model);
    }
}
