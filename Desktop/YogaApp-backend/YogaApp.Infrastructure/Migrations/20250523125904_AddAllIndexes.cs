using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPrograms_Users_UserId",
                table: "UserPrograms");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserPrograms",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_Category",
                table: "Workouts",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_Name",
                table: "Workouts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_WorkoutId",
                table: "Workouts",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrograms_UserId_ProgramId",
                table: "UserPrograms",
                columns: new[] { "UserId", "ProgramId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGeneralStats_UserId",
                table: "UserGeneralStats",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramName",
                table: "Programs",
                column: "ProgramName");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name",
                table: "Exercises",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CustomWorkouts_Category",
                table: "CustomWorkouts",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_CustomWorkouts_Difficulty",
                table: "CustomWorkouts",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_CustomWorkouts_Name",
                table: "CustomWorkouts",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPrograms_Users_UserId",
                table: "UserPrograms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPrograms_Users_UserId",
                table: "UserPrograms");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_Category",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_Name",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_WorkoutId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Name",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserPrograms_UserId_ProgramId",
                table: "UserPrograms");

            migrationBuilder.DropIndex(
                name: "IX_UserGeneralStats_UserId",
                table: "UserGeneralStats");

            migrationBuilder.DropIndex(
                name: "IX_Programs_ProgramName",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_CustomWorkouts_Category",
                table: "CustomWorkouts");

            migrationBuilder.DropIndex(
                name: "IX_CustomWorkouts_Difficulty",
                table: "CustomWorkouts");

            migrationBuilder.DropIndex(
                name: "IX_CustomWorkouts_Name",
                table: "CustomWorkouts");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserPrograms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPrograms_Users_UserId",
                table: "UserPrograms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
