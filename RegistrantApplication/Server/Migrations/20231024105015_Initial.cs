using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrantApplication.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountRoles",
                columns: table => new
                {
                    IdRole = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanLogin = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewAccounts = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanCreateAccounts = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditAccount = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanChangeRoles = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDeleteAccounts = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewRoles = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanCreateRoles = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditRoles = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDeleteRoles = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewAutos = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanCreateAutos = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditAutos = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDeleteAutos = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewDocuments = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanCreateDocuments = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditDocuments = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDeleteDocuments = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewContragents = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanCreateContragents = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditContragents = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDeleteContragents = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewOrders = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanCreateOrders = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditOrders = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDeleteOrders = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewOrderDetails = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanCreateOrderDetails = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEditOrderDetails = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDeleteOrderDetails = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanViewLogs = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "AccountsFileDocuments",
                columns: table => new
                {
                    IdFile = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    DataBytes = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsFileDocuments", x => x.IdFile);
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
                name: "Accounts",
                columns: table => new
                {
                    IdAccount = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Family = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Patronymic = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    AccountRoleIdRole = table.Column<long>(type: "INTEGER", nullable: false),
                    IsEmployee = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.IdAccount);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountRoles_AccountRoleIdRole",
                        column: x => x.AccountRoleIdRole,
                        principalTable: "AccountRoles",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountsAutos",
                columns: table => new
                {
                    IdAuto = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    AutoNumber = table.Column<string>(type: "TEXT", nullable: false),
                    AccountIdAccount = table.Column<long>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsAutos", x => x.IdAuto);
                    table.ForeignKey(
                        name: "FK_AccountsAutos_Accounts_AccountIdAccount",
                        column: x => x.AccountIdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountsDocuments",
                columns: table => new
                {
                    IdDocument = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Serial = table.Column<string>(type: "TEXT", nullable: true),
                    Number = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfIssue = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Authority = table.Column<string>(type: "TEXT", nullable: true),
                    AccountIdAccount = table.Column<long>(type: "INTEGER", nullable: true),
                    FileDocumentIdFile = table.Column<long>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsDocuments", x => x.IdDocument);
                    table.ForeignKey(
                        name: "FK_AccountsDocuments_AccountsFileDocuments_FileDocumentIdFile",
                        column: x => x.FileDocumentIdFile,
                        principalTable: "AccountsFileDocuments",
                        principalColumn: "IdFile",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountsDocuments_Accounts_AccountIdAccount",
                        column: x => x.AccountIdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountsSessions",
                columns: table => new
                {
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    AccountIdAccount = table.Column<long>(type: "INTEGER", nullable: false),
                    DateTimeSessionStarted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateTimeSessionExpired = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FingerPrintIdentity = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsSessions", x => x.Token);
                    table.ForeignKey(
                        name: "FK_AccountsSessions_Accounts_AccountIdAccount",
                        column: x => x.AccountIdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    IdEvent = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTimeEvent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AccountIdAccount = table.Column<long>(type: "INTEGER", nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false),
                    ValueObject = table.Column<string>(type: "TEXT", nullable: true),
                    ValueBefore = table.Column<string>(type: "TEXT", nullable: true),
                    ValueAfter = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.IdEvent);
                    table.ForeignKey(
                        name: "FK_Events_Accounts_AccountIdAccount",
                        column: x => x.AccountIdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    IdOrderDetails = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumRealese = table.Column<string>(type: "TEXT", nullable: true),
                    CountPodons = table.Column<string>(type: "TEXT", nullable: true),
                    PacketDocuments = table.Column<string>(type: "TEXT", nullable: true),
                    TochkaLoad = table.Column<string>(type: "TEXT", nullable: true),
                    Nomenclature = table.Column<string>(type: "TEXT", nullable: true),
                    Size = table.Column<string>(type: "TEXT", nullable: true),
                    Destination = table.Column<string>(type: "TEXT", nullable: true),
                    TypeLoad = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
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
                name: "Orders",
                columns: table => new
                {
                    IdOrder = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContragentIdContragent = table.Column<long>(type: "INTEGER", nullable: true),
                    AccountIdAccount = table.Column<long>(type: "INTEGER", nullable: true),
                    AutoIdAuto = table.Column<long>(type: "INTEGER", nullable: true),
                    DateTimeCreatedOrder = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateTimePlannedArrive = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateTimeRegistration = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateTimeArrived = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateTimeStartOrder = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateTimeEndOrder = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateTimeLeft = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OrderDetailsIdOrderDetails = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.IdOrder);
                    table.ForeignKey(
                        name: "FK_Orders_AccountsAutos_AutoIdAuto",
                        column: x => x.AutoIdAuto,
                        principalTable: "AccountsAutos",
                        principalColumn: "IdAuto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Accounts_AccountIdAccount",
                        column: x => x.AccountIdAccount,
                        principalTable: "Accounts",
                        principalColumn: "IdAccount",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Contragents_ContragentIdContragent",
                        column: x => x.ContragentIdContragent,
                        principalTable: "Contragents",
                        principalColumn: "IdContragent",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderDetails_OrderDetailsIdOrderDetails",
                        column: x => x.OrderDetailsIdOrderDetails,
                        principalTable: "OrderDetails",
                        principalColumn: "IdOrderDetails",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AccountRoles",
                columns: new[] { "IdRole", "CanChangeRoles", "CanCreateAccounts", "CanCreateAutos", "CanCreateContragents", "CanCreateDocuments", "CanCreateOrderDetails", "CanCreateOrders", "CanCreateRoles", "CanDeleteAccounts", "CanDeleteAutos", "CanDeleteContragents", "CanDeleteDocuments", "CanDeleteOrderDetails", "CanDeleteOrders", "CanDeleteRoles", "CanEditAccount", "CanEditAutos", "CanEditContragents", "CanEditDocuments", "CanEditOrderDetails", "CanEditOrders", "CanEditRoles", "CanLogin", "CanViewAccounts", "CanViewAutos", "CanViewContragents", "CanViewDocuments", "CanViewLogs", "CanViewOrderDetails", "CanViewOrders", "CanViewRoles", "IsDefault", "Title" },
                values: new object[] { 1L, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, true, "Гость" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountRoleIdRole",
                table: "Accounts",
                column: "AccountRoleIdRole");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsAutos_AccountIdAccount",
                table: "AccountsAutos",
                column: "AccountIdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsDocuments_AccountIdAccount",
                table: "AccountsDocuments",
                column: "AccountIdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsDocuments_FileDocumentIdFile",
                table: "AccountsDocuments",
                column: "FileDocumentIdFile");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsSessions_AccountIdAccount",
                table: "AccountsSessions",
                column: "AccountIdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Events_AccountIdAccount",
                table: "Events",
                column: "AccountIdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_StoreKeeperIdAccount",
                table: "OrderDetails",
                column: "StoreKeeperIdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AccountIdAccount",
                table: "Orders",
                column: "AccountIdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AutoIdAuto",
                table: "Orders",
                column: "AutoIdAuto");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ContragentIdContragent",
                table: "Orders",
                column: "ContragentIdContragent");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDetailsIdOrderDetails",
                table: "Orders",
                column: "OrderDetailsIdOrderDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsDocuments");

            migrationBuilder.DropTable(
                name: "AccountsSessions");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "AccountsFileDocuments");

            migrationBuilder.DropTable(
                name: "AccountsAutos");

            migrationBuilder.DropTable(
                name: "Contragents");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountRoles");
        }
    }
}
