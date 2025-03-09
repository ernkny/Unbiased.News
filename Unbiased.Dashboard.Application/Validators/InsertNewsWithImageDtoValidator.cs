using FluentValidation;
using Unbiased.Dashboard.Domain.Dto_s;

namespace Unbiased.Dashboard.Application.Validators
{
    public class InsertNewsWithImageDtoValidator : AbstractValidator<InsertNewsWithImageDto>
    {
        public InsertNewsWithImageDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must be less than or equal to 100 characters");

            RuleFor(x => x.Detail)
                .NotEmpty().WithMessage("Content is required");

            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required");
        }
    }
}
