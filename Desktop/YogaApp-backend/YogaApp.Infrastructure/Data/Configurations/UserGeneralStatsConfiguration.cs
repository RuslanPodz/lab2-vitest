using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.User.UserProgress;

namespace YogaApp.Infrastructure.Data.Configurations;

public class UserGeneralStatsConfiguration : IEntityTypeConfiguration<UserGeneralStatsModel>
{
    public void Configure(EntityTypeBuilder<UserGeneralStatsModel> builder)
    {
        //PrimaryKey
        builder.HasKey(ugs => ugs.UserId);
        
        //Properties
        builder.Property(ugs => ugs.TotalMinutes).IsRequired();
        builder.Property(ugs => ugs.TotalWorkouts).IsRequired();
        builder.Property(ugs => ugs.TotalCaloriesBurned).IsRequired();
        builder.Property(ugs => ugs.CurrentWorkoutStreak).IsRequired();
        builder.Property(ugs => ugs.LongestWorkoutStreak).IsRequired();
        
        //Relations
        builder.HasOne(ugs => ugs.User)
            .WithOne(u => u.GeneralStats)
            .HasForeignKey<UserGeneralStatsModel>(ugs => ugs.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}