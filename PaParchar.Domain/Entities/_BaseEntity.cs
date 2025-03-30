using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaParchar.Domain.Entities
{
    public abstract class _BaseEntity<ID> where ID : notnull
    {
        [Key]
        [Column("id")]
        public ID Id { get; set; } = default!;
    }
}
