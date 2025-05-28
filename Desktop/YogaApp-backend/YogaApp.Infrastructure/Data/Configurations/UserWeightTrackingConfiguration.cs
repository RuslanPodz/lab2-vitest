using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.User.UserProgress;

namespace YogaApp.Infrastructure.Data.Configurations;

public class UserWeightTrackingConfiguration : IEntityTypeConfiguration<UserWeightTrackingModel>
{
    public void Configure(EntityTypeBuilder<UserWeightTrackingModel> builder)
    {
        //PrimaryKey
        builder.HasKey(ut => ut.UserId);
        
        //Properties
        builder.Property(uwt => uwt.StartWeight).IsRequired();
        builder.Property(uwt => uwt.CurrentWeight).IsRequired();
        builder.Property(uwt => uwt.Height).IsRequired();
        
        //Relations
        builder.HasOne(uwt => uwt.User)
            .WithOne(u => u.WeightTracking)
            .HasForeignKey<UserWeightTrackingModel>(uwt => uwt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}