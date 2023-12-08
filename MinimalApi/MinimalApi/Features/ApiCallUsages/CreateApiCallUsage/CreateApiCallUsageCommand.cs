using System.Text.Json;
using ErrorOr;
using FluentValidation;
using MapsterMapper;
using MediatR;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.ApiCallUsages;

public sealed record CreateApiCallUsageCommand(
    EndpointFilterInvocationContext Context,
    EndpointFilterFactoryContext FilterFactoryContext,
    long ElapsedMilliseconds)
    : IRequest<ErrorOr<ApiCallUsageDto>>;

public sealed class CreateApiCallUsageCommandValidator : AbstractValidator<CreateApiCallUsageCommand>
{
    public CreateApiCallUsageCommandValidator()
    { }
}

public sealed class CreateApiCallUsageCommandHandler(IMapper mapper, IBaseCrudRepo<ApiCallUsage, Guid> apiCallUsageRepo) 
    : IRequestHandler<CreateApiCallUsageCommand, ErrorOr<ApiCallUsageDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IBaseCrudRepo<ApiCallUsage, Guid> _apiCallUsageRepo = apiCallUsageRepo;
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public async Task<ErrorOr<ApiCallUsageDto>> Handle(CreateApiCallUsageCommand commandRequest, CancellationToken cancellationToken)
    {
        var apiCallUsageDto = ApiCallUsageDto.Create(commandRequest.Context, commandRequest.FilterFactoryContext, commandRequest.ElapsedMilliseconds);
        _apiCallUsageRepo.Insert(_mapper.Map<ApiCallUsage>(apiCallUsageDto));
        await _apiCallUsageRepo.SaveAsync().ConfigureAwait(false);
        return apiCallUsageDto;
    }
}