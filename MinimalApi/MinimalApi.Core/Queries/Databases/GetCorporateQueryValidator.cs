using FluentValidation;

namespace MinimalApi.App.Queries.Databases;

public class GetCorporateQueryValidator : AbstractValidator<GetCorporateQuery>
{
    public GetCorporateQueryValidator()
    {
        RuleFor(x => x.DatabaseSchemaType).NotEmpty();
    }
}
