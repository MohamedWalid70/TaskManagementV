using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Presistence.DatabaseConfigurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasOne<UserEntity>().WithMany().HasForeignKey(rt => rt.UserId);
            builder.HasIndex(rt => rt.Token).IsUnique();
            builder.Property(rt => rt.Token).HasMaxLength(150);
        }
    }
}
