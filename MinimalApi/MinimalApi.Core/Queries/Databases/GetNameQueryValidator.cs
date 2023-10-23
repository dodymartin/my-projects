using FluentValidation;

namespace MinimalApi.App.Queries.Databases;

public class GetNameQueryValidator : AbstractValidator<GetNameQuery>
{
    public GetNameQueryValidator()
    {
        RuleFor(x => x.FacilityId).NotEmpty();
    }
}
