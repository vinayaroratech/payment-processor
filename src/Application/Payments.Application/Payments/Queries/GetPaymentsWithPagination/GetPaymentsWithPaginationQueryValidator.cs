using FluentValidation;

namespace Payments.Application.Payments.Queries.GetPaymentsWithPagination
{
    public class GetPaymentsWithPaginationQueryValidator : AbstractValidator<GetPaymentsWithPaginationQuery>
    {
        public GetPaymentsWithPaginationQueryValidator()
        {
            RuleFor(x => x.SearchText)
                .NotNull()
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}
