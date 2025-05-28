using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.User;
using YogaApp.Core.Domain.User.UserProgress;

namespace YogaApp.Infrastructure.Data.Configurations;

public class UserModelConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        //PrimaryKey
        builder.HasKey(u => u.UserId);
        
        //Properties
        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(250);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(250);
        
        //Indexes
        builder.HasIndex(u => u.Email).IsUnique();
        
        //Relations
        builder
            .HasMany(u => u.UserPrograms)
            .WithOne(up => up.User)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.UserWorkouts)
            .WithOne(uw => uw.User)
            .HasForeignKey(uw => uw.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.CustomWorkouts)
            .WithOne(cw => cw.User)
            .HasForeignKey(cw => cw.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(u => u.GeneralStats)
            .WithOne(gs => gs.User)
            .HasForeignKey<UserGeneralStatsModel>(gs => gs.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(u => u.WeightTracking)
            .WithOne(wt => wt.User)
            .HasForeignKey<UserWeightTrackingModel>(wt => wt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}