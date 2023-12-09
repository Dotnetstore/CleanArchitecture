using Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfigurations.Common;

public class BaseAuditableEntityEntityTypeConfiguration<T> : BaseEntityEntityTypeConfiguration<T> where T : BaseAuditableEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        
        builder
            .Property(q => q.CreatedBy)
            .IsRequired(false);
        
        builder
            .Property(q => q.CreatedDate)
            .IsRequired();
        
        builder
            .Property(q => q.LastUpdatedBy)
            .IsRequired(false);
        
        builder
            .Property(q => q.LastUpdatedDate)
            .IsRequired(false);
        
        builder
            .Property(q => q.DeletedBy)
            .IsRequired(false);
        
        builder
            .Property(q => q.DeletedDate)
            .IsRequired(false);
        
        builder
            .Property(q => q.IsDeleted)
            .IsRequired();
        
        builder
            .Property(q => q.IsSystem)
            .IsRequired();
        
        builder
            .Property(q => q.IsGdpr)
            .IsRequired();
    }
}