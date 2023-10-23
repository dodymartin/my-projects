using FluentValidation;

namespace MinimalApi.App.Queries.WebApis;

public class GetPingUrisQueryValidator : AbstractValidator<GetPingUrisQuery>
{
    public GetPingUrisQueryValidator()
    {
        RuleFor(x => x.ServiceNames).NotEmpty();
    }
}
