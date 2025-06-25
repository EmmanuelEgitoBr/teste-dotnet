using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Entities;

public class Idempotencia
{
    [Column("chave_idempotencia")]
    [Required]
    [StringLength(37)]
    public string ChaveIdempotencia { get; set; } = string.Empty;
    
    [Column("requisicao")]
    [Required]
    [StringLength(1000)]
    public string Requisicao { get; set; } = string.Empty;
    
    [Column("resultado")]
    [Required]
    [StringLength(1000)]
    public string Resultado { get; set; } = string.Empty;
}

