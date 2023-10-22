using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrantApplication.Server.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Accounts_AccountIdAccount",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Documents");

            migrationBuilder.RenameTable(
                name: "Sessions",
                newName: "AccountsSessions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Documents",
                newName: "DateOfIssue");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_AccountIdAccount",
                table: "AccountsSessions",
                newName: "IX_AccountsSessions_AccountIdAccount");

            migrationBuilder.AddColumn<string>(
                name: "Authority",
                table: "Documents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileDocumentIdFile",
                table: "Documents",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Documents",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Documents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "Documents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Autos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Autos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountsSessions",
                table: "AccountsSessions",
                column: "Token");

            migrationBuilder.CreateTable(
                name: "FileDocument",
                columns: table => new
                {
                    IdFile = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    DataBytes = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDocument", x => x.IdFile);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FileDocumentIdFile",
                table: "Documents",
                column: "FileDocumentIdFile");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsSessions_Accounts_AccountIdAccount",
                table: "AccountsSessions",
                column: "AccountIdAccount",
                principalTable: "Accounts",
                principalColumn: "IdAccount",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_FileDocument_FileDocumentIdFile",
                table: "Documents",
                column: "FileDocumentIdFile",
                principalTable: "FileDocument",
                principalColumn: "IdFile",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsSessions_Accounts_AccountIdAccount",
                table: "AccountsSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_FileDocument_FileDocumentIdFile",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "FileDocument");

            migrationBuilder.DropIndex(
                name: "IX_Documents_FileDocumentIdFile",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountsSessions",
                table: "AccountsSessions");

            migrationBuilder.DropColumn(
                name: "Authority",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FileDocumentIdFile",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Autos");

            migrationBuilder.RenameTable(
                name: "AccountsSessions",
                newName: "Sessions");

            migrationBuilder.RenameColumn(
                name: "DateOfIssue",
                table: "Documents",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_AccountsSessions_AccountIdAccount",
                table: "Sessions",
                newName: "IX_Sessions_AccountIdAccount");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Documents",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Autos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions",
                column: "Token");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Accounts_AccountIdAccount",
                table: "Sessions",
                column: "AccountIdAccount",
                principalTable: "Accounts",
                principalColumn: "IdAccount",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
