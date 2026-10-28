using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlotixPlatform.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCommerceBoundedContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "iot_catalog_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sku = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    icon_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_iot_catalog_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "iot_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_id = table.Column<int>(type: "integer", nullable: false),
                    sku = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    device_quantity = table.Column<int>(type: "integer", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    order_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_iot_orders", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "iot_catalog_items",
                columns: new[] { "id", "created_at", "icon_name", "name", "sku", "unit_price", "updated_at" },
                values: new object[,]
                {
                    { 1, null, "pin", "Rastreador GPS Flotix", "gps", 149.9m, null },
                    { 2, null, "signal", "Odómetro digital Flotix", "odometer", 89.9m, null },
                    { 3, null, "fuel", "Sensor de combustible Flotix", "fuel-sensor", 119.9m, null },
                    { 4, null, "chip", "Lector de diagnóstico OBD-II Flotix", "obd-ii", 99.9m, null },
                    { 5, null, "camera", "Cámara de cabina Flotix", "dashcam", 189.9m, null },
                    { 6, null, "gear", "Sensor de presión de llantas Flotix", "tpms", 129.9m, null },
                    { 7, null, "shield", "Inmovilizador remoto Flotix", "immobilizer", 169.9m, null },
                    { 8, null, "bell", "Botón de pánico Flotix", "panic-button", 59.9m, null },
                    { 9, null, "chart", "Sensor de temperatura de carga Flotix", "temp-sensor", 109.9m, null },
                    { 10, null, "users", "Lector de identificación de conductor Flotix", "driver-id", 79.9m, null }
                });

            migrationBuilder.CreateIndex(
                name: "i_x_iot_catalog_items_sku",
                table: "iot_catalog_items",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_iot_orders_owner_id",
                table: "iot_orders",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "iot_catalog_items");

            migrationBuilder.DropTable(
                name: "iot_orders");
        }
    }
}
