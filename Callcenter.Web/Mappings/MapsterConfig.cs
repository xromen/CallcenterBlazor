using Callcenter.Shared;
using Callcenter.Web.Models;
using Mapster;

namespace Callcenter.Web.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<GetRkListDto, DeclarationListDto>
            .NewConfig()
            .Map(dest => dest.Number,
                src => src.RkNumber)
            .Map(dest => dest.Theme,
                src => src.ObrTheme);

        TypeAdapterConfig<DeclarationModel, DeclarationDto>
            .NewConfig()
            .Map(dest => dest.MoPhoneNumber,
                src => src.MoPhone.PhoneNumber);
        
        TypeAdapterConfig<DateTime, DateOnly>.NewConfig()
            .MapWith(src => DateOnly.FromDateTime(src));
        
        TypeAdapterConfig<DateTime?, DateOnly?>.NewConfig()
            .MapWith(src => src.HasValue ? DateOnly.FromDateTime(src.Value) : null);

        TypeAdapterConfig<DateOnly, DateTime>.NewConfig()
            .MapWith(src => src.ToDateTime(TimeOnly.MinValue));
        TypeAdapterConfig<DateOnly?, DateTime?>.NewConfig()
            .MapWith(src => src.HasValue ? src.Value.ToDateTime(TimeOnly.MinValue) : null);
    }
}