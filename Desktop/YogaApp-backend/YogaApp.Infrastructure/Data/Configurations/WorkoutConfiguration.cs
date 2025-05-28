using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Infrastructure.Data.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<WorkoutModel>
{
    public void Configure(EntityTypeBuilder<WorkoutModel> builder)
    {
        builder.HasKey(w => w.WorkoutId);

        builder.Property(w => w.Name).IsRequired().HasMaxLength(100);
        builder.Property(w => w.Duration).IsRequired();
        builder.Property(w => w.Category).IsRequired().HasMaxLength(50);

        builder
            .HasMany(w => w.Exercises)
            .WithMany(e => e.Workouts);

        builder
            .HasMany(w => w.Programs)
            .WithMany(p => p.Workouts);
        
        builder
            .HasMany(w => w.UserWorkouts)
            .WithOne(uw => uw.Workout)
            .HasForeignKey(uw => uw.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}