using Microsoft.EntityFrameworkCore.Migrations;

namespace User.DAL.Migrations
{
    public partial class Fourth_Attempt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectedPeople_Person_PersonId",
                table: "ConnectedPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_City_CityId",
                table: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConnectedPeople",
                table: "ConnectedPeople");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "TP_Person");

            migrationBuilder.RenameTable(
                name: "ConnectedPeople",
                newName: "TP_ConnectedPeople");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "TP_City");

            migrationBuilder.RenameIndex(
                name: "IX_Person_CityId",
                table: "TP_Person",
                newName: "IX_TP_Person_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectedPeople_PersonId",
                table: "TP_ConnectedPeople",
                newName: "IX_TP_ConnectedPeople_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TP_Person",
                table: "TP_Person",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TP_ConnectedPeople",
                table: "TP_ConnectedPeople",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TP_City",
                table: "TP_City",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TP_ConnectedPeople_TP_Person_PersonId",
                table: "TP_ConnectedPeople",
                column: "PersonId",
                principalTable: "TP_Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TP_Person_TP_City_CityId",
                table: "TP_Person",
                column: "CityId",
                principalTable: "TP_City",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TP_ConnectedPeople_TP_Person_PersonId",
                table: "TP_ConnectedPeople");

            migrationBuilder.DropForeignKey(
                name: "FK_TP_Person_TP_City_CityId",
                table: "TP_Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TP_Person",
                table: "TP_Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TP_ConnectedPeople",
                table: "TP_ConnectedPeople");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TP_City",
                table: "TP_City");

            migrationBuilder.RenameTable(
                name: "TP_Person",
                newName: "Person");

            migrationBuilder.RenameTable(
                name: "TP_ConnectedPeople",
                newName: "ConnectedPeople");

            migrationBuilder.RenameTable(
                name: "TP_City",
                newName: "City");

            migrationBuilder.RenameIndex(
                name: "IX_TP_Person_CityId",
                table: "Person",
                newName: "IX_Person_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_TP_ConnectedPeople_PersonId",
                table: "ConnectedPeople",
                newName: "IX_ConnectedPeople_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConnectedPeople",
                table: "ConnectedPeople",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectedPeople_Person_PersonId",
                table: "ConnectedPeople",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_City_CityId",
                table: "Person",
                column: "CityId",
                principalTable: "City",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
