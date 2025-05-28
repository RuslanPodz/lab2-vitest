using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    IsLiked = table.Column<bool>(type: "boolean", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Difficulty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Password = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    WorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsFavourite = table.Column<bool>(type: "boolean", nullable: false),
                    LastPerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CaloriesBurned = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.WorkoutId);
                });

            migrationBuilder.CreateTable(
                name: "CustomWorkouts",
                columns: table => new
                {
                    CustomWorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Category = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    CaloriesBurned = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomWorkouts", x => x.CustomWorkoutId);
                    table.ForeignKey(
                        name: "FK_CustomWorkouts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    ProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    ProgramDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProgramCategory = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProgramImage = table.Column<string>(type: "text", nullable: true),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramId);
                    table.ForeignKey(
                        name: "FK_Programs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserGeneralStats",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalWorkouts = table.Column<int>(type: "integer", nullable: false),
                    TotalMinutes = table.Column<int>(type: "integer", nullable: true),
                    TotalCaloriesBurned = table.Column<int>(type: "integer", nullable: true),
                    CurrentWorkoutStreak = table.Column<int>(type: "integer", nullable: false),
                    LongestWorkoutStreak = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGeneralStats", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserGeneralStats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWeightTrackings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartWeight = table.Column<double>(type: "double precision", nullable: false),
                    CurrentWeight = table.Column<double>(type: "double precision", nullable: false),
                    Height = table.Column<double>(type: "double precision", nullable: false),
                    BMIStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeightTrackings", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserWeightTrackings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseModelWorkoutModel",
                columns: table => new
                {
                    ExercisesExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutsWorkoutId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseModelWorkoutModel", x => new { x.ExercisesExerciseId, x.WorkoutsWorkoutId });
                    table.ForeignKey(
                        name: "FK_ExerciseModelWorkoutModel_Exercises_ExercisesExerciseId",
                        column: x => x.ExercisesExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseModelWorkoutModel_Workouts_WorkoutsWorkoutId",
                        column: x => x.WorkoutsWorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "WorkoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkouts",
                columns: table => new
                {
                    UserWorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    CaloriesBurned = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsFavourite = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkouts", x => x.UserWorkoutId);
                    table.ForeignKey(
                        name: "FK_UserWorkouts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkouts_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "WorkoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomWorkoutModelExerciseModel",
                columns: table => new
                {
                    CustomWorkoutsCustomWorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExercisesExerciseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomWorkoutModelExerciseModel", x => new { x.CustomWorkoutsCustomWorkoutId, x.ExercisesExerciseId });
                    table.ForeignKey(
                        name: "FK_CustomWorkoutModelExerciseModel_CustomWorkouts_CustomWorkou~",
                        column: x => x.CustomWorkoutsCustomWorkoutId,
                        principalTable: "CustomWorkouts",
                        principalColumn: "CustomWorkoutId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomWorkoutModelExerciseModel_Exercises_ExercisesExercise~",
                        column: x => x.ExercisesExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramModelWorkoutModel",
                columns: table => new
                {
                    ProgramsProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutsWorkoutId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramModelWorkoutModel", x => new { x.ProgramsProgramId, x.WorkoutsWorkoutId });
                    table.ForeignKey(
                        name: "FK_ProgramModelWorkoutModel_Programs_ProgramsProgramId",
                        column: x => x.ProgramsProgramId,
                        principalTable: "Programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramModelWorkoutModel_Workouts_WorkoutsWorkoutId",
                        column: x => x.WorkoutsWorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "WorkoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPrograms",
                columns: table => new
                {
                    UserProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrograms", x => x.UserProgramId);
                    table.ForeignKey(
                        name: "FK_UserPrograms_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPrograms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomWorkoutModelExerciseModel_ExercisesExerciseId",
                table: "CustomWorkoutModelExerciseModel",
                column: "ExercisesExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomWorkouts_UserId",
                table: "CustomWorkouts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseModelWorkoutModel_WorkoutsWorkoutId",
                table: "ExerciseModelWorkoutModel",
                column: "WorkoutsWorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramModelWorkoutModel_WorkoutsWorkoutId",
                table: "ProgramModelWorkoutModel",
                column: "WorkoutsWorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_UserId",
                table: "Programs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrograms_ProgramId",
                table: "UserPrograms",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrograms_UserId",
                table: "UserPrograms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkouts_UserId",
                table: "UserWorkouts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkouts_WorkoutId",
                table: "UserWorkouts",
                column: "WorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomWorkoutModelExerciseModel");

            migrationBuilder.DropTable(
                name: "ExerciseModelWorkoutModel");

            migrationBuilder.DropTable(
                name: "ProgramModelWorkoutModel");

            migrationBuilder.DropTable(
                name: "UserGeneralStats");

            migrationBuilder.DropTable(
                name: "UserPrograms");

            migrationBuilder.DropTable(
                name: "UserWeightTrackings");

            migrationBuilder.DropTable(
                name: "UserWorkouts");

            migrationBuilder.DropTable(
                name: "CustomWorkouts");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
