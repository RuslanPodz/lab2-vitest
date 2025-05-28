using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Core.Domain.User;

namespace YogaApp.Infrastructure.Data.Configurations;

public class UserProgramConfiguration : IEntityTypeConfiguration<UserProgramModel>
{
    public void Configure(EntityTypeBuilder<UserProgramModel> builder)
    {
        //PrimaryKey
        builder.HasKey(up => up.UserProgramId);
        
        //Properties
        builder.Property(up => up.UserName).IsRequired().HasMaxLength(50);
        
        //Relations
        builder.HasOne(up => up.User)
            .WithMany(u => u.UserPrograms) // Користувач має список програм
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 Relations: UserProgram → Program
        builder.HasOne(up => up.Program)
            .WithMany(p => p.UserPrograms) // Програма має список учасників
            .HasForeignKey(up => up.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}