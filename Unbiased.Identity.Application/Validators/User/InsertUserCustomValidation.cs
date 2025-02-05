using FluentValidation;
using MediatR;
using Unbiased.Identity.Domain.Dto_s;

namespace Unbiased.Identity.Application.Validators.User
{
    public class InsertUserCustomValidation : AbstractValidator<InsertUserWithRolesDto>
    {

        public InsertUserCustomValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\?\*\.@\#\$\%\^\&\+\=]").WithMessage("Password must contain at least one special character (!?*.@#$%^&+=).");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
                .Matches("^[a-zA-Z0-9_.-]*$").WithMessage("Username can only contain letters, numbers, and the following special characters: . _ -");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Biography)
                .NotEmpty().WithMessage("Bio is required.")
                .MaximumLength(50).WithMessage("Bio must not exceed 2500 characters.");
        }
    }
}
