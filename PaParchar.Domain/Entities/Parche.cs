using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaParchar.Domain.Entities
{
    public class Parche
        : _BaseEntity<Guid>
    {
        [Key]
        [Column("parche_id")]
        public Guid ParcheId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [StringLength(500)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [StringLength(200)]
        [Column("ubicacion")]
        public string? Ubicacion { get; set; }

        [Column("duracion")]
        public int? Duracion { get; set; }

        [Column("capacidad")]
        public int? Capacidad { get; set; }

        [Column("es_privado")]
        public bool EsPrivado { get; set; } = false;

        [Column("precio")]
        [Precision(10, 2)] 
        public decimal? Precio { get; set; }

        [Column("portada_url")]
        [StringLength(500)]
        public string? PortadaUrl { get; set; }

        [Column("latitud")]
        [Precision(18, 15)]
        public decimal? Latitud { get; set; }

        [Column("longitud")]
        [Precision(18, 15)]
        public decimal? Longitud { get; set; }

        [Column("estado")]
        public EstadoParche Estado { get; set; } = EstadoParche.Creado;

        [Required]
        [Column("fecha_parche")]
        public DateTime FechaParche { get; set; }

        [Required]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("ultima_actualizacion")]
        public DateTime? UltimaActualizacion { get; set; }
    }

    public enum EstadoParche
    {
        Creado,
        Planificado,
        EnProgreso,
        Completado,
        Cancelado
    }
}