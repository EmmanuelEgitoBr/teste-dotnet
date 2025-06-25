using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Entities;

public class ContaCorrente
{
    [Column("idcontacorrente")]
    [Required]
    [StringLength(37)]
    public string IdContaCorrente { get; set; } = string.Empty;
    [Column("numero")]
    [Required]
    public int Numero { get; set; }
    [Column("nome")]
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;
    [Column("ativo")]
    [Required]
    public bool Ativo { get; set; }
}

