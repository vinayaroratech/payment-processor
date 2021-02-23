using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Common.Models;
using Payments.Domain.Events;

namespace Payments.Application.Payments.EventHandlers
{
    public class PaymentCompletedEventHandler : NotificationHandler<DomainEventNotification<PaymentCompletedEvent>>
    {
        private readonly ILogger<PaymentCompletedEventHandler> _logger;

        public PaymentCompletedEventHandler(ILogger<PaymentCompletedEventHandler> logger)
        {
            _logger = logger;
        }

        protected override void Handle(DomainEventNotification<PaymentCompletedEvent> notification)
        {
            var domainEvent = notification.DomainEvent;
            _logger.LogInformation("Domain event - {domainEvent} handler called", domainEvent.GetType().Name);
        }
    }
}