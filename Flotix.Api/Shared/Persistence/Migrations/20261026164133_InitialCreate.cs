using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flotix.Api.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "monitoring");

            migrationBuilder.EnsureSchema(
                name: "fleet");

            migrationBuilder.EnsureSchema(
                name: "store");

            migrationBuilder.EnsureSchema(
                name: "maintenance");

            migrationBuilder.EnsureSchema(
                name: "notifications");

            migrationBuilder.EnsureSchema(
                name: "reporting");

            migrationBuilder.EnsureSchema(
                name: "iam");

            migrationBuilder.CreateTable(
                name: "fleet_preferences",
                schema: "fleet",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    speed_limit_kmh = table.Column<int>(type: "integer", nullable: false),
                    maintenance_interval_km = table.Column<int>(type: "integer", nullable: false),
                    fuel_anomaly_drop_pct = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fleet_preferences", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "iot_catalog_item",
                schema: "store",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    sku = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric", nullable: false),
                    icon_name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iot_catalog_item", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "iam",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    license_number = table.Column<string>(type: "text", nullable: true),
                    license_expiry = table.Column<DateOnly>(type: "date", nullable: true),
                    company_name = table.Column<string>(type: "text", nullable: true),
                    workshop_name = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "iot_order",
                schema: "store",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<string>(type: "text", nullable: false),
                    sku = table.Column<string>(type: "text", nullable: false),
                    device_quantity = table.Column<int>(type: "integer", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    order_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iot_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_iot_order_iot_catalog_item_sku",
                        column: x => x.sku,
                        principalSchema: "store",
                        principalTable: "iot_catalog_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_iot_order_user_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notification_preference",
                schema: "notifications",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    maintenance = table.Column<bool>(type: "boolean", nullable: false),
                    speed = table.Column<bool>(type: "boolean", nullable: false),
                    incidents = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_preference", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_preference_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "report",
                schema: "reporting",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<string>(type: "text", nullable: false),
                    report_type = table.Column<string>(type: "text", nullable: false),
                    generated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    filter_params = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report", x => x.id);
                    table.ForeignKey(
                        name: "FK_report_user_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicle",
                schema: "fleet",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<string>(type: "text", nullable: false),
                    driver_id = table.Column<string>(type: "text", nullable: true),
                    plate = table.Column<string>(type: "text", nullable: false),
                    vehicle_type = table.Column<string>(type: "text", nullable: false),
                    current_mileage = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.id);
                    table.ForeignKey(
                        name: "FK_vehicle_user_driver_id",
                        column: x => x.driver_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicle_user_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workshop",
                schema: "maintenance",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    mechanic_id = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    district = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<decimal>(type: "numeric", nullable: false),
                    tags = table.Column<List<string>>(type: "text[]", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workshop", x => x.id);
                    table.ForeignKey(
                        name: "FK_workshop_user_mechanic_id",
                        column: x => x.mechanic_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "alert",
                schema: "monitoring",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    vehicle_id = table.Column<string>(type: "text", nullable: false),
                    alert_type = table.Column<string>(type: "text", nullable: false),
                    activated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert", x => x.id);
                    table.ForeignKey(
                        name: "FK_alert_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "fleet",
                        principalTable: "vehicle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "fuel_record",
                schema: "fleet",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    vehicle_id = table.Column<string>(type: "text", nullable: false),
                    driver_id = table.Column<string>(type: "text", nullable: false),
                    liters = table.Column<decimal>(type: "numeric", nullable: false),
                    total_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    mileage_at_refuel = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fuel_record", x => x.id);
                    table.ForeignKey(
                        name: "FK_fuel_record_user_driver_id",
                        column: x => x.driver_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fuel_record_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "fleet",
                        principalTable: "vehicle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "incident",
                schema: "fleet",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    vehicle_id = table.Column<string>(type: "text", nullable: false),
                    driver_id = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    reported_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incident", x => x.id);
                    table.ForeignKey(
                        name: "FK_incident_user_driver_id",
                        column: x => x.driver_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_incident_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "fleet",
                        principalTable: "vehicle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "iot_device",
                schema: "monitoring",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    vehicle_id = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    connection_status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iot_device", x => x.id);
                    table.ForeignKey(
                        name: "FK_iot_device_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "fleet",
                        principalTable: "vehicle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "maintenance_request",
                schema: "maintenance",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    vehicle_id = table.Column<string>(type: "text", nullable: false),
                    mechanic_id = table.Column<string>(type: "text", nullable: true),
                    fault_description = table.Column<string>(type: "text", nullable: false),
                    estimated_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_maintenance_request_user_mechanic_id",
                        column: x => x.mechanic_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_maintenance_request_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "fleet",
                        principalTable: "vehicle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                schema: "notifications",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    alert_id = table.Column<string>(type: "text", nullable: true),
                    message = table.Column<string>(type: "text", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_alert_alert_id",
                        column: x => x.alert_id,
                        principalSchema: "monitoring",
                        principalTable: "alert",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notification_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "iam",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "telemetry_data",
                schema: "monitoring",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    device_id = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telemetry_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_telemetry_data_iot_device_device_id",
                        column: x => x.device_id,
                        principalSchema: "monitoring",
                        principalTable: "iot_device",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "fleet",
                table: "fleet_preferences",
                columns: new[] { "id", "created_at", "fuel_anomaly_drop_pct", "maintenance_interval_km", "speed_limit_kmh" },
                values: new object[] { "default", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 30, 10000, 90 });

            migrationBuilder.InsertData(
                schema: "store",
                table: "iot_catalog_item",
                columns: new[] { "id", "created_at", "icon_name", "name", "sku", "unit_price" },
                values: new object[,]
                {
                    { "dashcam", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "camera", "Cámara de cabina Flotix", "dashcam", 189.9m },
                    { "driver-id", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "users", "Lector de identificación de conductor Flotix", "driver-id", 79.9m },
                    { "fuel-sensor", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "fuel", "Sensor de combustible Flotix", "fuel-sensor", 119.9m },
                    { "gps", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "pin", "Rastreador GPS Flotix", "gps", 149.9m },
                    { "immobilizer", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "shield", "Inmovilizador remoto Flotix", "immobilizer", 169.9m },
                    { "obd-ii", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "chip", "Lector de diagnóstico OBD-II Flotix", "obd-ii", 99.9m },
                    { "odometer", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "signal", "Odómetro digital Flotix", "odometer", 89.9m },
                    { "panic-button", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "bell", "Botón de pánico Flotix", "panic-button", 59.9m },
                    { "temp-sensor", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "chart", "Sensor de temperatura de carga Flotix", "temp-sensor", 109.9m },
                    { "tpms", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "gear", "Sensor de presión de llantas Flotix", "tpms", 129.9m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_alert_vehicle_id",
                schema: "monitoring",
                table: "alert",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_fuel_record_driver_id",
                schema: "fleet",
                table: "fuel_record",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_fuel_record_vehicle_id",
                schema: "fleet",
                table: "fuel_record",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_incident_driver_id",
                schema: "fleet",
                table: "incident",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_incident_vehicle_id",
                schema: "fleet",
                table: "incident",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_iot_catalog_item_sku",
                schema: "store",
                table: "iot_catalog_item",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iot_device_serial_number",
                schema: "monitoring",
                table: "iot_device",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iot_device_vehicle_id",
                schema: "monitoring",
                table: "iot_device",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_iot_order_owner_id",
                schema: "store",
                table: "iot_order",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_iot_order_sku",
                schema: "store",
                table: "iot_order",
                column: "sku");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_request_mechanic_id",
                schema: "maintenance",
                table: "maintenance_request",
                column: "mechanic_id");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_request_vehicle_id",
                schema: "maintenance",
                table: "maintenance_request",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_alert_id",
                schema: "notifications",
                table: "notification",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_user_id",
                schema: "notifications",
                table: "notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_preference_user_id",
                schema: "notifications",
                table: "notification_preference",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_report_owner_id",
                schema: "reporting",
                table: "report",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_telemetry_data_device_id",
                schema: "monitoring",
                table: "telemetry_data",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                schema: "iam",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_driver_id",
                schema: "fleet",
                table: "vehicle",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_owner_id",
                schema: "fleet",
                table: "vehicle",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_plate",
                schema: "fleet",
                table: "vehicle",
                column: "plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workshop_mechanic_id",
                schema: "maintenance",
                table: "workshop",
                column: "mechanic_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fleet_preferences",
                schema: "fleet");

            migrationBuilder.DropTable(
                name: "fuel_record",
                schema: "fleet");

            migrationBuilder.DropTable(
                name: "incident",
                schema: "fleet");

            migrationBuilder.DropTable(
                name: "iot_order",
                schema: "store");

            migrationBuilder.DropTable(
                name: "maintenance_request",
                schema: "maintenance");

            migrationBuilder.DropTable(
                name: "notification",
                schema: "notifications");

            migrationBuilder.DropTable(
                name: "notification_preference",
                schema: "notifications");

            migrationBuilder.DropTable(
                name: "report",
                schema: "reporting");

            migrationBuilder.DropTable(
                name: "telemetry_data",
                schema: "monitoring");

            migrationBuilder.DropTable(
                name: "workshop",
                schema: "maintenance");

            migrationBuilder.DropTable(
                name: "iot_catalog_item",
                schema: "store");

            migrationBuilder.DropTable(
                name: "alert",
                schema: "monitoring");

            migrationBuilder.DropTable(
                name: "iot_device",
                schema: "monitoring");

            migrationBuilder.DropTable(
                name: "vehicle",
                schema: "fleet");

            migrationBuilder.DropTable(
                name: "user",
                schema: "iam");
        }
    }
}
