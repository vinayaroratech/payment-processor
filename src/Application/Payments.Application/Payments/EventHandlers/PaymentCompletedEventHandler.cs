using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Common.Models;
using Payments.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.EventHandlers
{
    public class PaymentCompletedEventHandler : INotificationHandler<DomainEventNotification<PaymentCompletedEvent>>
    {
        private readonly ILogger<PaymentCompletedEventHandler> _logger;

        public PaymentCompletedEventHandler(ILogger<PaymentCompletedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<PaymentCompletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            _logger.LogInformation("Domain event - {domainEvent} handler called", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}