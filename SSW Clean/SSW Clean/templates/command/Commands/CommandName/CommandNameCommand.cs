using SSW_Clean.Application.Common.Interfaces;

namespace SSW_Clean.Application.Features.EntityNames.Commands.CommandName;

public record CommandNameCommand() : IRequest<Guid>;

public class CommandNameCommandHandler : IRequestHandler<CommandNameCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public CommandNameCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CommandNameCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add your business logic and persistence here

        throw new NotImplementedException();
    }
}
