using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contacting.API.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "contacting");

            migrationBuilder.CreateTable(
                name: "persons",
                schema: "contacting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: true),
                    company = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_persons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "requests",
                schema: "contacting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_request", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "personcontacts",
                schema: "contacting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_type = table.Column<byte>(type: "smallint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    person_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_person_contacts", x => x.id);
                    table.ForeignKey(
                        name: "fk_person_contacts_persons_person_id",
                        column: x => x.person_id,
                        principalSchema: "contacting",
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_person_contacts_person_id",
                schema: "contacting",
                table: "personcontacts",
                column: "person_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "personcontacts",
                schema: "contacting");

            migrationBuilder.DropTable(
                name: "requests",
                schema: "contacting");

            migrationBuilder.DropTable(
                name: "persons",
                schema: "contacting");
        }
    }
}
