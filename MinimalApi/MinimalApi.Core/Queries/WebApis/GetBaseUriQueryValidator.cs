using FluentValidation;

namespace MinimalApi.App.Queries.WebApis;

public class GetBaseUriQueryValidator : AbstractValidator<GetBaseUriQuery>
{
    public GetBaseUriQueryValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}
