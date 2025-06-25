namespace Questao5.Application.Dtos;

public class ConsultaSaldoDto
{
    public int Numero { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataConsulta { get; set; }
    public decimal Saldo { get; set; }
}
