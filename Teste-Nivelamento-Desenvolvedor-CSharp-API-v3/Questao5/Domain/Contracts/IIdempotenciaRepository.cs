namespace Questao5.Domain.Contracts;

public interface IIdempotenciaRepository
{
    Task<string?> ObterResultadoAsync(string chave);
    Task RegistrarAsync(string chave, object requisicao, object resultado);
}
