using FluentValidation;

namespace MinimalApi.App.Queries.Applications;

public class CheckMinimumVersionQueryValidator : AbstractValidator<CheckMinimumVersionQuery>
{
    public CheckMinimumVersionQueryValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty()
            .Unless(x => !string.IsNullOrEmpty(x.ApplicationName));
        RuleFor(x => x.ApplicationName).NotEmpty()
            .Unless(x => x.ApplicationId.HasValue && x.ApplicationId.Value > 0);
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}
