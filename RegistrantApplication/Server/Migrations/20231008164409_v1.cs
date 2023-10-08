using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrantApplication.Server.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    IdAccount = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Family = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Patronymic = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.IdAccount);
                });

            migrationBuilder.CreateTable(
                name: "Contragents",
                columns: table => new
                {
                    IdContragent = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contragents", x => x.IdContragent);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    IdDriver = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Family = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Patronymic = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.IdDriver);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    IdOrderDetails = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumRealese = table.Column<string>(type: "TEXT", nullable: false),
                    CountPodons = table.Column<string>(type: "TEXT", nullable: false),
                    PacketDocuments = table.Column<string>(type: "TEXT", nullable: false),
                    TochkaLoad = table.Column<string>(type: "TEXT", nullable: false),
                    Nomenclature = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    Destination = table.Column<string>(type: "TEXT", nullable: false),
                    TypeLoad = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    StoreKeeperIdAccount = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.IdOrderDetails);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Accounts_StoreKeeperIdAccount",
                        column: x => x.StoreKeeperIdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Autos",
                columns: table => new
                {
                    IdAuto = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    AutoNumber = table.Column<string>(type: "TEXT", nullable: false),
                    DriverIdDriver = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autos", x => x.IdAuto);
                    table.ForeignKey(
                        name: "FK_Autos_Drivers_DriverIdDriver",
                        column: x => x.DriverIdDriver,
                        principalTable: "Drivers",
                        principalColumn: "IdDriver",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    IdDocument = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false),
                    AccountIdAccount = table.Column<long>(type: "INTEGER", nullable: true),
                    DriverIdDriver = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.IdDocument);
                    table.ForeignKey(
                        name: "FK_Documents_Accounts_AccountIdAccount",
                        column: x => x.AccountIdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Drivers_DriverIdDriver",
                        column: x => x.DriverIdDriver,
                        principalTable: "Drivers",
                        principalColumn: "IdDriver",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    IdOrder = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContragentIdContragent = table.Column<long>(type: "INTEGER", nullable: true),
                    DriverIdDriver = table.Column<long>(type: "INTEGER", nullable: true),
                    AutoIdAuto = table.Column<long>(type: "INTEGER", nullable: true),
                    DateTimeCreatedOrder = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateTimePlannedArrive = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateTimeRegistration = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateTimeArrived = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateTimeStartOrder = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateTimeEndOrder = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateTimeLeft = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.IdOrder);
                    table.ForeignKey(
                        name: "FK_Orders_Autos_AutoIdAuto",
                        column: x => x.AutoIdAuto,
                        principalTable: "Autos",
                        principalColumn: "IdAuto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Contragents_ContragentIdContragent",
                        column: x => x.ContragentIdContragent,
                        principalTable: "Contragents",
                        principalColumn: "IdContragent",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Drivers_DriverIdDriver",
                        column: x => x.DriverIdDriver,
                        principalTable: "Drivers",
                        principalColumn: "IdDriver",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Autos_DriverIdDriver",
                table: "Autos",
                column: "DriverIdDriver");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AccountIdAccount",
                table: "Documents",
                column: "AccountIdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DriverIdDriver",
                table: "Documents",
                column: "DriverIdDriver");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_StoreKeeperIdAccount",
                table: "OrderDetails",
                column: "StoreKeeperIdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AutoIdAuto",
                table: "Orders",
                column: "AutoIdAuto");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ContragentIdContragent",
                table: "Orders",
                column: "ContragentIdContragent");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DriverIdDriver",
                table: "Orders",
                column: "DriverIdDriver");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Autos");

            migrationBuilder.DropTable(
                name: "Contragents");

            migrationBuilder.DropTable(
                name: "Drivers");
        }
    }
}
