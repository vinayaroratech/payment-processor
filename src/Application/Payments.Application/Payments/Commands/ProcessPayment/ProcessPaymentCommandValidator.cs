using FluentValidation;
using Payments.Application.Common.Interfaces;
using System;

namespace Payments.Application.Payments.Commands.ProcessPayment
{
    public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
    {
        private readonly IApplicationDbContext _context;
        public ProcessPaymentCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.CreditCardNumber)
                .Length(16).WithMessage("Credit Card Number must be 16 digits.")
                .NotEmpty().WithMessage("Credit Card Number is required.");

            RuleFor(v => v.CardHolder)
                .NotEmpty().WithMessage("Card Holder is required.");

            RuleFor(v => v.ExpirationDate.Date)
                .NotEmpty().WithMessage("Expiration Date is required.")
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Expiration Date can not be in past.")
                .OverridePropertyName("ExpirationDate");

            RuleFor(v => v.SecurityCode)
                 .Length(3).WithMessage("Security Code must be 3 characters.")
                 .When(w => !string.IsNullOrWhiteSpace(w.SecurityCode));

            RuleFor(v => v.Amount)
                .NotEmpty().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        }
    }
}
