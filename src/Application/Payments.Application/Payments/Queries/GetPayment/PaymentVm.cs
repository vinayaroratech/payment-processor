using Payments.Application.Common.Mappings;
using Payments.Domain.Entities;

namespace Payments.Application.Payments.Queries.GetPayment
{
    public class PaymentVm : IMapFrom<Payment>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }

    }
}