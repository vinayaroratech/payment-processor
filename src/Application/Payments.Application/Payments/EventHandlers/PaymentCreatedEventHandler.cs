using MediatR;
using Microsoft.Extensions.Logging;
using Payments.Application.Common.Models;
using Payments.Domain.Events;

namespace Payments.Application.Payments.EventHandlers
{
    public class PaymentCreatedEventHandler : NotificationHandler<DomainEventNotification<PaymentCreatedEvent>>
    {
        private readonly ILogger<PaymentCreatedEventHandler> _logger;

        public PaymentCreatedEventHandler(ILogger<PaymentCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        protected override void Handle(DomainEventNotification<PaymentCreatedEvent> notification)
        {
            var domainEvent = notification.DomainEvent;
            _logger.LogInformation("Domain event - {domainEvent} handler called", domainEvent.GetType().Name);
        }
    }
}