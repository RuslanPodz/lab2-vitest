using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBmiColumnToUserWeightTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BMI",
                table: "UserWeightTrackings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DifferenceInWeight",
                table: "UserWeightTrackings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BMI",
                table: "UserWeightTrackings");

            migrationBuilder.DropColumn(
                name: "DifferenceInWeight",
                table: "UserWeightTrackings");
        }
    }
}
