using FluentValidation;

namespace MinimalApi.Endpoints;

public class DatabaseRequest
{
    public string ChildDatabaseName { get; set; }
    public int? FacilityId { get; set; }
}

public class DatabaseRequestValidator : AbstractValidator<DatabaseRequest>
{
    public DatabaseRequestValidator()
    {
        RuleSet("ChildName", () =>
        {
            RuleFor(x => x.ChildDatabaseName).NotEmpty();
        });
        RuleSet("Facility", () =>
        {
            RuleFor(x => x.FacilityId).NotEmpty();
        });
    }
}
