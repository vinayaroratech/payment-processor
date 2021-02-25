using System;
using System.Threading.Tasks;

namespace Payments.Application.Services.PaymentGateway
{
    public interface IPaymentService
    {
        Task<long> MakePaymentAsync<T>(T model) where T : IPaymentModel;
        bool AppliesTo<T>(T model) where T : IPaymentModel;
    }
}
