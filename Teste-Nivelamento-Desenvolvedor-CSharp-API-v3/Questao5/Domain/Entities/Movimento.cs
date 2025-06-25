using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Entities;

[Table("movimento")]
public class Movimento
{
    [Key]
    [Column("idmovimento")]
    [StringLength(37)]
    public string IdMovimento { get; set; } = string.Empty;

    [Column("idcontacorrente")]
    [Required]
    [StringLength(37)]
    public string IdContaCorrente { get; set; } = string.Empty;

    [Column("datamovimento")]
    [Required]
    public DateTime DataMovimento { get; set; }

    [Column("tipomovimento")]
    [Required]
    [StringLength(1)]
    public string TipoMovimento { get; set; } = string.Empty;

    [Column("valor")]
    [Required]
    public decimal Valor { get; set; }
}
