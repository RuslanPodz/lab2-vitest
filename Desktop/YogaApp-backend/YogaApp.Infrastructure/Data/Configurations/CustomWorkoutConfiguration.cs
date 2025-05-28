using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Infrastructure.Data.Configurations;

public class CustomWorkoutConfiguration : IEntityTypeConfiguration<CustomWorkoutModel>
{
    public void Configure(EntityTypeBuilder<CustomWorkoutModel> builder)
    {
        builder.HasKey(cw => cw.CustomWorkoutId);

        builder.Property(cw => cw.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cw => cw.Duration)
            .IsRequired();

        builder.Property(cw => cw.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(cw => cw.UserId);
        builder.HasIndex(cw => cw.Category);

        builder.HasOne(cw => cw.User)
            .WithMany(u => u.CustomWorkouts)
            .HasForeignKey(cw => cw.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(cw => cw.Exercises)
            .WithMany(e => e.CustomWorkouts)
            .UsingEntity(j => j.ToTable("CustomWorkoutExercises"));
    }
}