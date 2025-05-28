using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.User;

namespace YogaApp.Infrastructure.Data.Configurations;

public class UserWorkoutConfiguration : IEntityTypeConfiguration<UserWorkoutModel>
{
    public void Configure(EntityTypeBuilder<UserWorkoutModel> builder)
    {
        //PrimaryKey
        builder.HasKey(uw => uw.UserWorkoutId);
        
        //Properties
        builder.Property(uw => uw.Duration).IsRequired();
        builder.Property(uw => uw.Status).IsRequired();
        builder.Property(uw => uw.CaloriesBurned).IsRequired();
        
        builder.HasOne(uw => uw.User)
            .WithMany(u => u.UserWorkouts)
            .HasForeignKey(uw => uw.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uw => uw.Workout)
            .WithMany(w => w.UserWorkouts)
            .HasForeignKey(uw => uw.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}