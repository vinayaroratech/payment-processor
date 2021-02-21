using System.Collections.Generic;

namespace Payments.Application.Payments.Queries.GetPaymentsList
{
    public class PaymentsListVm
    {
        public IList<PaymentDto> Payments { get; set; }
    }
}