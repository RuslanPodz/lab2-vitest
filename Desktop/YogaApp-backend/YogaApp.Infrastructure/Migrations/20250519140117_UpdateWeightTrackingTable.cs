using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWeightTrackingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWeightTrackings",
                table: "UserWeightTrackings");

            migrationBuilder.AddColumn<Guid>(
                name: "WeightTrackingId",
                table: "UserWeightTrackings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWeightTrackings",
                table: "UserWeightTrackings",
                column: "WeightTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWeightTrackings_UserId",
                table: "UserWeightTrackings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWeightTrackings",
                table: "UserWeightTrackings");

            migrationBuilder.DropIndex(
                name: "IX_UserWeightTrackings_UserId",
                table: "UserWeightTrackings");

            migrationBuilder.DropColumn(
                name: "WeightTrackingId",
                table: "UserWeightTrackings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWeightTrackings",
                table: "UserWeightTrackings",
                column: "UserId");
        }
    }
}
