using Payments.Application.Common.Mappings;
using Payments.Domain.Entities;

namespace Payments.Application.Payments.Queries.GetPaymentsList
{
    public class PaymentDto : IMapFrom<Payment>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public string Status { get; set; }
    }
}