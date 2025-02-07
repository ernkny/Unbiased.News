using FluentValidation;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Application.Validators.User
{
    public class UpdateUserCustomValidation : AbstractValidator<UpdateUserWithRolesDto>
    {

        public UpdateUserCustomValidation()
        {

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Biography)
                .NotEmpty().WithMessage("Bio is required.")
                .MaximumLength(50).WithMessage("Bio must not exceed 2500 characters.");

            RuleFor(x => x.Roles).NotEmpty().WithMessage("At least one role is required.");
        }
    }
}
