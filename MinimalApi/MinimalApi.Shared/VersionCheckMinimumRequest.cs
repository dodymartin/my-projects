using FluentValidation;

namespace MinimalApi.Shared;

public class VersionCheckMinimumRequest
{
    public int? ApplicationId { get; set; }
    public string ApplicationName { get; set; }
    public string ApplicationVersion { get; set; }
    public int? FacilityId { get; set; }
}

public class VersionCheckMinimumRequestValidator : AbstractValidator<VersionCheckMinimumRequest>
{
    public VersionCheckMinimumRequestValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty()
            .Unless(x => !string.IsNullOrEmpty(x.ApplicationName));
        RuleFor(x => x.ApplicationName).NotEmpty()
            .Unless(x => x.ApplicationId.HasValue && x.ApplicationId.Value > 0);
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}
