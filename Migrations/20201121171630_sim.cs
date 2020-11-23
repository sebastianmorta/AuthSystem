using Microsoft.EntityFrameworkCore.Migrations;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Migrations
{
    public partial class sim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "IoTDevices",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoffeeNessessaryForCoffee",
                table: "IoTDevices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaterNessessaryForCoffee",
                table: "IoTDevices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoffeeNessessaryForCoffee",
                table: "IoTDevices");

            migrationBuilder.DropColumn(
                name: "WaterNessessaryForCoffee",
                table: "IoTDevices");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "IoTDevices",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}
