using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaParchar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateParche : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parches",
                columns: table => new
                {
                    parche_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ubicacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    duracion = table.Column<int>(type: "integer", nullable: true),
                    capacidad = table.Column<int>(type: "integer", nullable: true),
                    es_privado = table.Column<bool>(type: "boolean", nullable: false),
                    precio = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    portada_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(18,15)", precision: 18, scale: 15, nullable: true),
                    longitud = table.Column<decimal>(type: "numeric(18,15)", precision: 18, scale: 15, nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    fecha_parche = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ultima_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parches", x => x.parche_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parches");
        }
    }
}
