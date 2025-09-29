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
        
        TypeAdapterConfig<User, UserDto>
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
        
        TypeAdapterConfig<Declaration, DeclarationDto>
            .NewConfig()
            .Map(dest => dest.StatusName,
                src => src.Status.Name)
            .Map(dest => dest.CodeName,
                src => src.Code.OrganisationName.Name)
            .Map(dest => dest.TypeName,
                src => src.Type.Name)
            .Map(dest => dest.ContactFormName,
                src => src.ContactForm.Name)
            .Map(dest => dest.AnswerStatusName,
                src => src.AnswerStatus.Name)
            .Map(dest => dest.KemTypeName,
                src => src.KemType.Name)
            .Map(dest => dest.AnswerOrgName,
                src => src.AnswerOrg.Name)
            .Map(dest => dest.SvoStatusName,
                src => src.SvoStatus.Name)
            .Map(dest => dest.CitizenCategoryName,
                src => src.CitizenCategory.Name)
            .Map(dest => dest.InsuredSmoName,
                src => src.InsuredSmo.Name)
            .Map(dest => dest.ResultName,
                src => src.Result.Name)
            .Map(dest => dest.SvedJalName,
                src => src.SvedJal.Name)
            .Map(dest => dest.HaveOrgName,
                src => src.HaveOrg.Name)
            .Map(dest => dest.MpTypeName,
                src => src.MpType.Name)
            .Map(dest => dest.RedirectReasonName,
                src => src.RedirectReason.Name);

        TypeAdapterConfig<DeclarationDto, Models.Excel.Declaration>
            .NewConfig()
            .Map(dest => dest.CreatorUserName,
                src => src.Creator.FullName)
            .Map(dest => dest.AnswerUserName,
                src => src.AnswerUser.FullName);

        TypeAdapterConfig<Declaration, Models.Excel.Declaration>
            .NewConfig()
            .Map(dest => dest.StatusName,
                src => src.Status.Name)
            .Map(dest => dest.CodeName,
                src => src.Code.OrganisationName.Name)
            .Map(dest => dest.TypeName,
                src => src.Type.Name)
            .Map(dest => dest.ContactFormName,
                src => src.ContactForm.Name)
            .Map(dest => dest.AnswerStatusName,
                src => src.AnswerStatus.Name)
            .Map(dest => dest.KemTypeName,
                src => src.KemType.Name)
            .Map(dest => dest.AnswerOrgName,
                src => src.AnswerOrg.Name)
            .Map(dest => dest.SvoStatusName,
                src => src.SvoStatus.Name)
            .Map(dest => dest.CitizenCategoryName,
                src => src.CitizenCategory.Name)
            .Map(dest => dest.InsuredSmoName,
                src => src.InsuredSmo.Name)
            .Map(dest => dest.ResultName,
                src => src.Result.Name)
            .Map(dest => dest.SvedJalName,
                src => src.SvedJal.Name)
            .Map(dest => dest.HaveOrgName,
                src => src.HaveOrg.Name)
            .Map(dest => dest.MpTypeName,
                src => src.MpType.Name)
            .Map(dest => dest.RedirectReasonName,
                src => src.RedirectReason.Name)
            .Map(dest => dest.CreatorUserName,
                src => src.Creator.FullName)
            .Map(dest => dest.AnswerUserName,
                src => src.AnswerUser.FullName);

        TypeAdapterConfig<News, NewsDto>
            .NewConfig()
            .Map(dest => dest.OrganisationIds,
                src => src.Organisations.Select(o => o.Id).ToList());
    }
}