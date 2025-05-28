using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.Program;

namespace YogaApp.Infrastructure.Data.Configurations;

public class ProgramConfiguration : IEntityTypeConfiguration<ProgramModel>
{
    public void Configure(EntityTypeBuilder<ProgramModel> builder)
    {
        builder.HasKey(p => p.ProgramId);

        builder.Property(p => p.ProgramName)
            .IsRequired(false)
            .HasMaxLength(60);

        builder.Property(p => p.ProgramDescription)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(p => p.ProgramCategory)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.ProgramImage)
            .IsRequired(false);

        builder.Property(p => p.Difficulty)
            .IsRequired();

        builder
            .HasMany(p => p.Workouts)
            .WithMany(w => w.Programs);

        builder
            .HasMany(p => p.UserPrograms)
            .WithOne(up => up.Program)
            .HasForeignKey(up => up.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}