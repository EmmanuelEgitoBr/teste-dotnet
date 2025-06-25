using Questao5.Domain.Entities;

namespace Questao5.Domain.Contracts.MovimentarConta;

public interface IMovimentoRepository
{
    Task<bool> MovimentoExisteAsync(string idMovimento);
    Task InserirMovimentoAsync(Movimento movimento);
}
