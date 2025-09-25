using Callcenter.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Callcenter.Api.Data.Configuration;

public class DeclarationConfiguration : IEntityTypeConfiguration<Declaration>
{
    public void Configure(EntityTypeBuilder<Declaration> builder)
    {
        var dateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v == Constants.NullDateTime ? null : v,
            v => v == Constants.NullDateTime ? null : v
        );
        var dateOnlyConverter = new ValueConverter<DateOnly?, DateOnly?>(
            v => v == Constants.NullDateOnly ? null : v,
            v => v == Constants.NullDateOnly ? null : v
        );
        var intConverter = new ValueConverter<int?, int?>(
            v => v == 0 ? null : v,
            v => v == 0 ? null : v
        );
        
        builder
            .HasOne(c => c.MoPhone)
            .WithMany()
            .HasPrincipalKey(c => c.PhoneNumber)
            .HasForeignKey(c => c.MoPhoneNumber);
        
        builder
            .Property(c => c.DateRegisteredSmo)
            .HasConversion(dateTimeConverter);
        
        builder
            .Property(c => c.CitizenCategoryId)
            .HasConversion(intConverter);
        
        builder
            .Property(c => c.DateRegistered)
            .HasConversion(dateTimeConverter);
        
        builder
            .Property(c => c.BirthDate)
            .HasConversion(dateOnlyConverter);
        
        builder
            .Property(c => c.AnswerDate)
            .HasConversion(dateOnlyConverter);
        
        builder
            .Property(c => c.ClosedDate)
            .HasConversion(dateOnlyConverter);
        
        builder
            .Property(c => c.SvedJalId)
            .HasConversion(intConverter);
        
        builder
            .Property(c => c.SupervisorDate)
            .HasConversion(dateOnlyConverter);
        
        builder
            .Property(c => c.SupervisorSmoDate)
            .HasConversion(dateOnlyConverter);
        
        builder
            .Property(c => c.RedirectReasonId)
            .HasConversion(intConverter);
    }
}