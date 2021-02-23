using Payments.Domain.Common;
using System.Threading.Tasks;

namespace Payments.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
