using FluentValidation;

namespace MinimalApi.App.Queries.WebApis;

public class GetControllerUrisQueryValidator : AbstractValidator<GetControllerUrisQuery>
{
    public GetControllerUrisQueryValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty()
            .Unless(x => !string.IsNullOrEmpty(x.ApplicationName));
        RuleFor(x => x.ApplicationName).NotEmpty()
            .Unless(x => x.ApplicationId.HasValue && x.ApplicationId.Value > 0);
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}
