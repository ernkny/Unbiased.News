using FluentValidation;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Validators
{
    /// <summary>
    ///  Validator for contact form submissions.
    /// </summary>
    public class ContactValidator : AbstractValidator<Contact>
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="ContactValidator"/> class.
        /// </summary>
        public ContactValidator()
        {
            RuleFor(contact => contact.Subject)
                .NotEmpty().WithMessage("Subject cannot be empty")
                .Length(1, 500).WithMessage("Subject must be between 1 and 500 characters");

            RuleFor(contact => contact.Message)
                .NotEmpty().WithMessage("Message cannot be empty");

            RuleFor(contact => contact.FullName)
                .NotEmpty().WithMessage("Full name cannot be empty")
                .Length(1, 500).WithMessage("Full name must be between 1 and 500 characters");

            RuleFor(contact => contact.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Email must be a valid email address")
                .Length(1, 100).WithMessage("Email must be between 1 and 100 characters");

        }
    }
}
