using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.Program;
using YogaApp.Core.Domain.User;
using YogaApp.Core.Domain.User.UserProgress;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<WorkoutModel> Workouts { get; set; }
    public DbSet<CustomWorkoutModel> CustomWorkouts { get; set; }
    public DbSet<ProgramModel> Programs { get; set; }
    public DbSet<ExerciseModel> Exercises { get; set; }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<UserWorkoutModel> UserWorkouts { get; set; }
    public DbSet<UserProgramModel> UserPrograms { get; set; }
    public DbSet<UserWeightTrackingModel> UserWeightTrackings { get; set; }
    public DbSet<UserGeneralStatsModel> UserGeneralStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        
        modelBuilder.Entity<WorkoutModel>().ToTable("Workouts");
        modelBuilder.Entity<CustomWorkoutModel>().ToTable("CustomWorkouts");
        modelBuilder.Entity<ProgramModel>().ToTable("Programs");
        modelBuilder.Entity<ExerciseModel>().ToTable("Exercises");
        modelBuilder.Entity<UserModel>().ToTable("Users");
        modelBuilder.Entity<UserWorkoutModel>().ToTable("UserWorkouts");
        modelBuilder.Entity<UserProgramModel>().ToTable("UserPrograms");
        modelBuilder.Entity<UserWeightTrackingModel>().ToTable("UserWeightTrackings");
        modelBuilder.Entity<UserGeneralStatsModel>().ToTable("UserGeneralStats");
        
        // --- Індекси для WorkoutModel ---
        modelBuilder.Entity<WorkoutModel>()
            .HasIndex(w => w.Name);
        modelBuilder.Entity<WorkoutModel>()
            .HasIndex(w => w.WorkoutId);
        modelBuilder.Entity<WorkoutModel>()
            .HasIndex(w => w.Category);

        // --- Індекси для CustomWorkoutModel ---
        modelBuilder.Entity<CustomWorkoutModel>()
            .HasIndex(c => c.Name);
        modelBuilder.Entity<CustomWorkoutModel>()
            .HasIndex(c => c.Category);
        modelBuilder.Entity<CustomWorkoutModel>()
            .HasIndex(c => c.Difficulty);

        // --- Індекси для ProgramModel ---
        modelBuilder.Entity<ProgramModel>()
            .HasIndex(p => p.ProgramName);

        // --- Індекси для ExerciseModel ---
        modelBuilder.Entity<ExerciseModel>()
            .HasIndex(e => e.Name);

        // --- Індекси для UserModel ---
        modelBuilder.Entity<UserModel>()
            .HasIndex(u => u.Email)
            .IsUnique(); // якщо email має бути унікальним
        modelBuilder.Entity<UserModel>()
            .HasIndex(u => u.Name);

        // --- Індекси для UserWorkoutModel ---
        modelBuilder.Entity<UserWorkoutModel>()
            .HasIndex(uw => uw.UserId);
        modelBuilder.Entity<UserWorkoutModel>()
            .HasIndex(uw => uw.WorkoutId);

        // --- Індекси для UserProgramModel ---
        modelBuilder.Entity<UserProgramModel>()
            .HasIndex(up => up.UserId);
        modelBuilder.Entity<UserProgramModel>()
            .HasIndex(up => up.ProgramId);
        modelBuilder.Entity<UserProgramModel>()
            .HasIndex(up => new { up.UserId, up.ProgramId }).IsUnique();

        // --- Індекси для UserWeightTrackingModel ---
        modelBuilder.Entity<UserWeightTrackingModel>()
            .HasIndex(wt => wt.UserId);

        // --- Індекси для UserGeneralStatsModel ---
        modelBuilder.Entity<UserGeneralStatsModel>()
            .HasIndex(gs => gs.UserId).IsUnique(); // один запис на користувача
    }
}