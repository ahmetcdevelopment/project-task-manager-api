using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class RoleOperationClaimConfiguration : IEntityTypeConfiguration<RoleOperationClaim>
{
    public void Configure(EntityTypeBuilder<RoleOperationClaim> builder)
    {
        builder.ToTable("RoleOperationClaims").HasKey(roc => roc.Id);

        builder.Property(roc => roc.Id).HasColumnName("Id").IsRequired();
        builder.Property(roc => roc.RoleId).HasColumnName("RoleId");
        builder.Property(roc => roc.OperationClaimId).HasColumnName("OperationClaimId");
        builder.Property(roc => roc.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(roc => roc.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(roc => roc.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(roc => !roc.DeletedDate.HasValue);
    }
}