using Microsoft.EntityFrameworkCore.Migrations;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Migrations
{
    public partial class x : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_AspNetUsers_ApplicationUserId",
                table: "Devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "Devices",
                newName: "IoTDevices");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_ApplicationUserId",
                table: "IoTDevices",
                newName: "IX_IoTDevices_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IoTDevices",
                table: "IoTDevices",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_IoTDevices_AspNetUsers_ApplicationUserId",
                table: "IoTDevices",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IoTDevices_AspNetUsers_ApplicationUserId",
                table: "IoTDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IoTDevices",
                table: "IoTDevices");

            migrationBuilder.RenameTable(
                name: "IoTDevices",
                newName: "Devices");

            migrationBuilder.RenameIndex(
                name: "IX_IoTDevices_ApplicationUserId",
                table: "Devices",
                newName: "IX_Devices_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_AspNetUsers_ApplicationUserId",
                table: "Devices",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
