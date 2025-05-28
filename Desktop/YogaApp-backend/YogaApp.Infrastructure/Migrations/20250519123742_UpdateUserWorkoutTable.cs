using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserWorkoutTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomWorkouts_Users_UserId",
                table: "CustomWorkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkouts_Users_UserId",
                table: "UserWorkouts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workouts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserWorkouts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPerformedAt",
                table: "UserWorkouts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CustomWorkouts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomWorkouts_Users_UserId",
                table: "CustomWorkouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkouts_Users_UserId",
                table: "UserWorkouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomWorkouts_Users_UserId",
                table: "CustomWorkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkouts_Users_UserId",
                table: "UserWorkouts");

            migrationBuilder.DropColumn(
                name: "LastPerformedAt",
                table: "UserWorkouts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workouts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserWorkouts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CustomWorkouts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomWorkouts_Users_UserId",
                table: "CustomWorkouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkouts_Users_UserId",
                table: "UserWorkouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
