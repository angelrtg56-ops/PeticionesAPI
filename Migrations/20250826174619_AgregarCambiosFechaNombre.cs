using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeticionesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCambiosFechaNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Peticiones",
                columns: table => new
                {
                    PeticionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeticionTexto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreOpcional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peticiones", x => x.PeticionID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Peticiones");
        }
    }
}
