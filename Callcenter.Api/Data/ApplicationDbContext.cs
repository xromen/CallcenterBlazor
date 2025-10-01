using Callcenter.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenIddict.EntityFrameworkCore.Models;

namespace Callcenter.Api.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Declaration> Declarations { get; set; } = null!;
    public DbSet<DeclarationAction> DeclarationHistories { get; set; } = null!;
    public DbSet<DeclarationPermission> DeclarationPermissions { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<QuestionGroup> QuestionGroups { get; set; } = null!;
    public DbSet<DeclarationFile> Files { get; set; } = null!;
    public DbSet<News> News { get; set; } = null!;
    public DbSet<NewsPermission> NewsPermissions { get; set; } = null!;
    public DbSet<UserNotification> UserNotifications { get; set; } = null!;
    public DbSet<SupervisorComment> SupervisorComments { get; set; } = null!;
    
    // Справочники
    public DbSet<UserGroup> UserGroups { get; set; } = null!;
    public DbSet<Organisation> Organisations { get; set; } = null!;
    public DbSet<AnswerStatus> AnswerStatuses { get; set; } = null!;
    public DbSet<CitizenCategory> CitizenCategories { get; set; } = null!;
    public DbSet<DeclarationContactForm> DeclarationContactForms { get; set; } = null!;
    public DbSet<DeclarationResult> DeclarationResults { get; set; } = null!;
    public DbSet<DeclarationSource> DeclarationSources { get; set; } = null!;
    public DbSet<DeclarationStatus> DeclarationStatuses { get; set; } = null!;
    public DbSet<DeclarationType> DeclarationTypes { get; set; } = null!;
    public DbSet<F003Mo> F003Mos { get; set; } = null!;
    public DbSet<F003MoMcod> F003MoMcods { get; set; } = null!;
    public DbSet<F003MoDocument> F003MoDocuments { get; set; } = null!;
    public DbSet<F003Document> F003Documents { get; set; } = null!;
    public DbSet<KemType> KemTypes { get; set; } = null!;
    public DbSet<MpType> MpTypes { get; set; } = null!;
    public DbSet<RedirectReason> RedirectReasons { get; set; } = null!;
    public DbSet<SvedJal> SvedJal { get; set; } = null!;
    public DbSet<SvoStatus> SvoStatuses { get; set; } = null!;
    public DbSet<OrganisationName> OrganisationNames { get; set; } = null!;
    public DbSet<MoPhoneNumber> MoPhoneNumbers { get; set; } = null!;
    public DbSet<DeclarationTheme> DeclarationThemes { get; set; } = null!;
    public DbSet<IdentityDocumentType> IdentityDocumentTypes { get; set; } = null!;
    public DbSet<MoDepartment> MoDepartments { get; set; } = null!;
    public DbSet<Region> Regions { get; set; } = null!;
    public DbSet<MoRegion> MoRegions { get; set; } = null!;
    
    public DbSet<NotificationType> NotificationTypes { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Для OpenIddict Authorizations
        modelBuilder.Entity<OpenIddictEntityFrameworkCoreAuthorization>()
            .Property(a => a.CreationDate)
            .HasColumnType("timestamp with time zone");

        // Аналогично если нужны ещё поля в OpenIddict
        modelBuilder.Entity<OpenIddictEntityFrameworkCoreToken>()
            .Property(t => t.CreationDate)
            .HasColumnType("timestamp with time zone");
        modelBuilder.Entity<OpenIddictEntityFrameworkCoreToken>()
            .Property(t => t.ExpirationDate)
            .HasColumnType("timestamp with time zone");
        modelBuilder.Entity<OpenIddictEntityFrameworkCoreToken>()
            .Property(t => t.RedemptionDate)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<News>()
            .HasMany(u => u.Organisations)
            .WithMany()
            .UsingEntity<NewsPermission>();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTime>()
            .HaveColumnType("timestamp without time zone");
    }
}