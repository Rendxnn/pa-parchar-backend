using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaParchar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddParcheAndHorario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "parches",
                columns: table => new
                {
                    parche_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ubicacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    capacidad = table.Column<int>(type: "integer", nullable: true),
                    es_privado = table.Column<bool>(type: "boolean", nullable: false),
                    precio = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    portada_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(18,15)", precision: 18, scale: 15, nullable: true),
                    longitud = table.Column<decimal>(type: "numeric(18,15)", precision: 18, scale: 15, nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ultima_actualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parches", x => x.parche_id);
                });

            migrationBuilder.CreateTable(
                name: "parches_horarios",
                columns: table => new
                {
                    horario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parche_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dia = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    hora_inicio = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    hora_fin = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parches_horarios", x => x.horario_id);
                    table.ForeignKey(
                        name: "FK_parches_horarios_parches_parche_id",
                        column: x => x.parche_id,
                        principalTable: "parches",
                        principalColumn: "parche_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_parches_horarios_parche_id",
                table: "parches_horarios",
                column: "parche_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parches_horarios");

            migrationBuilder.DropTable(
                name: "parches");
        }
    }
}
