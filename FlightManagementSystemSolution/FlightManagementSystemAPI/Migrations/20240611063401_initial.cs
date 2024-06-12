using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagementSystemAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_FlightId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Routes_RouteId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Cancellations_CancellationId",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Flights_FlightId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_SubRoutes_Flights_FlightId",
                table: "SubRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_SubRoutes_Routes_RouteId",
                table: "SubRoutes");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_FlightId",
                table: "Bookings",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Routes_RouteId",
                table: "Bookings",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Cancellations_CancellationId",
                table: "Refunds",
                column: "CancellationId",
                principalTable: "Cancellations",
                principalColumn: "CancellationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Flights_FlightId",
                table: "Routes",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubRoutes_Flights_FlightId",
                table: "SubRoutes",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubRoutes_Routes_RouteId",
                table: "SubRoutes",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_FlightId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Routes_RouteId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Cancellations_CancellationId",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Flights_FlightId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_SubRoutes_Flights_FlightId",
                table: "SubRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_SubRoutes_Routes_RouteId",
                table: "SubRoutes");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_FlightId",
                table: "Bookings",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Routes_RouteId",
                table: "Bookings",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Cancellations_CancellationId",
                table: "Refunds",
                column: "CancellationId",
                principalTable: "Cancellations",
                principalColumn: "CancellationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Flights_FlightId",
                table: "Routes",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubRoutes_Flights_FlightId",
                table: "SubRoutes",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubRoutes_Routes_RouteId",
                table: "SubRoutes",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
