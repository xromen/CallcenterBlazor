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
        
        TypeAdapterConfig<User, UserToSendDto>
            .NewConfig()
            .Map(dest => dest.GroupName,
                src => src.Group.Name)
            .Map(dest => dest.OrgName,
                src => src.Organisation.Name);
        
        TypeAdapterConfig<DeclarationAction, DeclarationActionDto>
            .NewConfig()
            .Map(dest => dest.UserFullName,
                src => src.User.FullName)
            .Map(dest => dest.UserGroupName,
                src => src.User.Group.Name);
    }
}