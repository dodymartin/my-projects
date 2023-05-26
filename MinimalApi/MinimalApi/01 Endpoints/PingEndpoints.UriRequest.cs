using FluentValidation;

namespace MinimalApi.Endpoints;

public class PingUriRequest
{
    public IEnumerable<string> ServiceNames { get; set; }
}

public class PingUriRequestValidator : AbstractValidator<PingUriRequest>
{
    public PingUriRequestValidator()
    {
        RuleFor(x => x.ServiceNames).NotEmpty();
    }
}
