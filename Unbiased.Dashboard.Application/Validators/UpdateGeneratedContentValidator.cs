using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Dto_s.Content;

namespace Unbiased.Dashboard.Application.Validators
{
    /// <summary>
    /// FluentValidation validator for UpdateAllContentDataRequest
    /// </summary>
    public class UpdateGeneratedContentValidator : AbstractValidator<UpdateAllContentDataRequest>
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="UpdateGeneratedContentValidator"/> class.
        /// </summary>
        public UpdateGeneratedContentValidator()
        {

            RuleFor(x => x.ContentSubHeadingId)
                .GreaterThan(0)
                .WithMessage("Content ID must be greater than 0");


            RuleFor(x => x.ContentCategoryId)
                .GreaterThan(0)
                .WithMessage("Please select a valid category");


            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(200)
                .WithMessage("Title cannot exceed 200 characters")
                .MinimumLength(3)
                .WithMessage("Title must be at least 3 characters");


            RuleFor(x => x.UniqUrlPath)
                .NotEmpty()
                .WithMessage("URL path is required")
                .MaximumLength(500)
                .WithMessage("URL path cannot exceed 500 characters")
                .WithMessage("URL path can only contain letters, numbers, hyphens, underscores and forward slashes");


            RuleFor(x => x.SubTitle)
                .MaximumLength(500)
                .WithMessage("Subtitle cannot exceed 500 characters");


            RuleFor(x => x.ImagePrompt)
                .MaximumLength(1000)
                .WithMessage("Image prompt cannot exceed 1000 characters");


            RuleFor(x => x.Hashtags)
                .MaximumLength(500)
                .WithMessage("Hashtags cannot exceed 500 characters")
                .Must(BeValidHashtags)
                .WithMessage("Hashtags must be separated by commas or spaces and each hashtag should start with #")
                .When(x => !string.IsNullOrEmpty(x.Hashtags));


            RuleFor(x => x.ImagePath)
                .MaximumLength(1000)
                .WithMessage("Image path cannot exceed 1000 characters");


            RuleFor(x => x.Questions)
                .NotNull()
                .WithMessage("Questions list cannot be null");

            RuleForEach(x => x.Questions)
                .SetValidator(new QuestionDtoValidator())
                .When(x => x.Questions != null);


            RuleFor(x => x.Steps)
                .NotNull()
                .WithMessage("Steps list cannot be null");

            RuleForEach(x => x.Steps)
                .SetValidator(new StepDtoValidator())
                .When(x => x.Steps != null);


            RuleFor(x => x.Steps)
                .Must(HaveUniqueStepNumbers)
                .WithMessage("Step numbers must be unique")
                .When(x => x.Steps != null && x.Steps.Any());


            RuleFor(x => x.CreatedTime)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Created time cannot be in the future")
                .When(x => x.CreatedTime.HasValue);
        }

        /// <summary>
        /// Validates that the hashtags are in the correct format.
        /// </summary>
        /// <param name="hashtags"></param>
        /// <returns></returns>
        private bool BeValidHashtags(string hashtags)
        {
            if (string.IsNullOrWhiteSpace(hashtags)) return true;


            var tags = hashtags.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return tags.All(tag => tag.Trim().StartsWith("#") && tag.Trim().Length > 1);
        }

        /// <summary>
        /// Validates that all step numbers are unique.
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        private bool HaveUniqueStepNumbers(List<StepDto> steps)
        {
            if (steps == null || !steps.Any()) return true;

            var stepNumbers = steps.Select(s => s.StepNumber).ToList();
            return stepNumbers.Count == stepNumbers.Distinct().Count();
        }
    }

    /// <summary>
    /// FluentValidation validator for QuestionDto
    /// </summary>
    public class QuestionDtoValidator : AbstractValidator<QuestionDto>
    {
        public QuestionDtoValidator()
        {

            RuleFor(x => x.Question)
                .NotEmpty()
                .WithMessage("Question is required")
                .MaximumLength(1000)
                .WithMessage("Question cannot exceed 1000 characters")
                .MinimumLength(5)
                .WithMessage("Question must be at least 5 characters");


            RuleFor(x => x.Answer)
                .NotEmpty()
                .WithMessage("Answer is required")
                .MaximumLength(2000)
                .WithMessage("Answer cannot exceed 2000 characters")
                .MinimumLength(10)
                .WithMessage("Answer must be at least 10 characters");
        }
    }

    /// <summary>
    /// FluentValidation validator for StepDto
    /// </summary>
    public class StepDtoValidator : AbstractValidator<StepDto>
    {
        public StepDtoValidator()
        {

            RuleFor(x => x.StepNumber)
                .GreaterThan(0)
                .WithMessage("Step number must be greater than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Step number cannot exceed 100");


            RuleFor(x => x.StepTitle)
                .NotEmpty()
                .WithMessage("Step title is required")
                .MaximumLength(200)
                .WithMessage("Step title cannot exceed 200 characters")
                .MinimumLength(3)
                .WithMessage("Step title must be at least 3 characters");


            RuleFor(x => x.StepDescription)
                .NotEmpty()
                .WithMessage("Step description is required")
                .MaximumLength(2000)
                .WithMessage("Step description cannot exceed 2000 characters")
                .MinimumLength(10)
                .WithMessage("Step description must be at least 10 characters");
        }
    }
}


