using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class DBv05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jwt_Users_UserID",
                table: "Jwt");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Address_AddressID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Jwt_UserID",
                table: "Jwt");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordSalt");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressID",
                table: "Users",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Jwt_UserID",
                table: "Jwt",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jwt_Users_UserID",
                table: "Jwt",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Address_AddressID",
                table: "Users",
                column: "AddressID",
                principalTable: "Address",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jwt_Users_UserID",
                table: "Jwt");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Address_AddressID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Jwt_UserID",
                table: "Jwt");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "Password");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressID",
                table: "Users",
                column: "AddressID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jwt_UserID",
                table: "Jwt",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Jwt_Users_UserID",
                table: "Jwt",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Address_AddressID",
                table: "Users",
                column: "AddressID",
                principalTable: "Address",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
