using FluentValidation;

namespace MinimalApi.Shared;

public class BaseUriRequest
{
    public int? ApplicationId { get; set; }
    public string? ApplicationVersion { get; set; }
}

public class BaseUriRequestValidator : AbstractValidator<BaseUriRequest>
{
    public BaseUriRequestValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}