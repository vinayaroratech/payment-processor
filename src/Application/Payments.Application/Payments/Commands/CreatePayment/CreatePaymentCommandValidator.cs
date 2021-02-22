using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreatePaymentCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
                .NotEmpty().WithMessage("Name is required.")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
            
        }

        public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .AllAsync(l => l.Name != name);
        }
    }
}
