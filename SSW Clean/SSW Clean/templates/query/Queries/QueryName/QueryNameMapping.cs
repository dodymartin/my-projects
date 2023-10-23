using SSW_Clean.Domain.Entities;

namespace SSW_Clean.Application.Features.EntityNames.Queries.QueryName;

public class QueryNameMapping : Profile
{
    public QueryNameMapping()
    {
        CreateMap<EntityName, EntityNameDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.Value));
    }
}
