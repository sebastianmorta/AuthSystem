using Microsoft.EntityFrameworkCore.Migrations;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Migrations
{
    public partial class xs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentGrainAmount",
                table: "IoTDevices",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentWaterAmount",
                table: "IoTDevices",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentGrainAmount",
                table: "IoTDevices");

            migrationBuilder.DropColumn(
                name: "CurrentWaterAmount",
                table: "IoTDevices");
        }
    }
}
