using Callcenter.Api.Data.Entities;
using Callcenter.Api.Models;
using Callcenter.Shared;
using Mapster;

namespace Callcenter.Api.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<GetRkListModel, GetRkListDto>
            .NewConfig()
            .Map(dest => dest.IsBad,
                src => src.IsBad == 1);
    }
}