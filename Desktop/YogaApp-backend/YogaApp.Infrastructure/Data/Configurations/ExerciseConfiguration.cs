using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Infrastructure.Data.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<ExerciseModel>
{
    public void Configure(EntityTypeBuilder<ExerciseModel> builder)
    {
        builder.HasKey(e => e.ExerciseId);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.ImageUrl)
            .IsRequired();

        builder.Property(e => e.Duration)
            .IsRequired();

        builder.Property(e => e.Difficulty)
            .IsRequired();

        // Many-to-many with WorkoutModel
        builder
            .HasMany(e => e.Workouts)
            .WithMany(w => w.Exercises)
            .UsingEntity(j => j.ToTable("WorkoutExercises"));

        // Many-to-many with CustomWorkoutModel
        builder
            .HasMany(e => e.CustomWorkouts)
            .WithMany(cw => cw.Exercises)
            .UsingEntity(j => j.ToTable("CustomWorkoutExercises"));
    }
}