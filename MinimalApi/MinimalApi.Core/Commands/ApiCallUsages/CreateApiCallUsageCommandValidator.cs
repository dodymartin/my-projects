using FluentValidation;

namespace MinimalApi.App.Commands.ApiCallUsages;

public class CreateApiCallUsageCommandValidator : AbstractValidator<CreateApiCallUsageCommand>
{
    public CreateApiCallUsageCommandValidator()
    { }
}
