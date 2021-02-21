using FluentValidation;

namespace Payments.Application.Payments.Commands.CreatePayment
{
    public class CreateTodoItemCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreateTodoItemCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
