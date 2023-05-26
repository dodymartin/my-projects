using FluentValidation;
using MinimalApi.Dal;

namespace MinimalApi.Endpoints;

public class ControllerUriRequest
{
    public MinimalApiDbContext DbContext { get; set; }

    public int? ApplicationId { get; set; }
    public string ApplicationName { get; set; }
    public string ApplicationVersion { get; set; }
    public string ControllerName { get; set; }
    public int? FacilityId { get; set; }
    public string MachineName { get; set; }
}

public class ControllerUrisRequestValidator : AbstractValidator<ControllerUriRequest>
{
    public ControllerUrisRequestValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty()
            .Unless(x => !string.IsNullOrEmpty(x.ApplicationName));
        RuleFor(x => x.ApplicationName).NotEmpty()
            .Unless(x => x.ApplicationId.HasValue && x.ApplicationId.Value > 0);
        RuleFor(x => x.ApplicationVersion).NotEmpty();
    }
}
