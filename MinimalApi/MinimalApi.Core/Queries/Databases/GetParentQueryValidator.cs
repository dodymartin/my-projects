using FluentValidation;

namespace MinimalApi.App.Queries.Databases;

public class GetParentQueryValidator : AbstractValidator<GetParentQuery>
{
    public GetParentQueryValidator()
    {
        RuleFor(x => x.ChildDatabaseName).NotEmpty();
    }
}
