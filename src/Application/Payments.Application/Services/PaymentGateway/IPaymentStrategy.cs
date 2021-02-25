using System.Threading.Tasks;

namespace Payments.Application.Services.PaymentGateway
{
    public interface IPaymentStrategy
    {
        Task<long> MakePaymentAsync<T>(T model) where T : IPaymentModel;
        IPaymentService GetPaymentService<TPaymentModel>(TPaymentModel model)
            where TPaymentModel : IPaymentModel;
    }
}
