using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_balance = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    age = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Adress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    passport = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    position = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    department = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    salary = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    contract = table.Column<string>(type: "text", nullable: false, defaultValue: "Контракт сотрудника по умолчанию"),
                    age = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Adress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    passport = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency_name = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "RUP"),
                    amount = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accounts_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_ClientId",
                table: "accounts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_clients_passport",
                table: "clients",
                column: "passport",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_phone_number",
                table: "clients",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_passport",
                table: "employees",
                column: "passport",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_phone_number",
                table: "employees",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
