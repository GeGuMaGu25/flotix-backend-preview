using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlotixPlatform.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFleetBoundedContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fleet_preferences",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    speed_limit_kmh = table.Column<int>(type: "integer", nullable: false),
                    maintenance_interval_km = table.Column<int>(type: "integer", nullable: false),
                    fuel_anomaly_drop_pct = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_fleet_preferences", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_id = table.Column<int>(type: "integer", nullable: false),
                    driver_id = table.Column<int>(type: "integer", nullable: true),
                    plate = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    vehicle_type = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    current_mileage = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vehicles", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "fleet_preferences",
                columns: new[] { "id", "created_at", "fuel_anomaly_drop_pct", "maintenance_interval_km", "speed_limit_kmh", "updated_at" },
                values: new object[] { 1, null, 30, 10000, 90, null });

            migrationBuilder.CreateIndex(
                name: "i_x_vehicles_driver_id",
                table: "vehicles",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "i_x_vehicles_owner_id",
                table: "vehicles",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "i_x_vehicles_plate",
                table: "vehicles",
                column: "plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fleet_preferences");

            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}
