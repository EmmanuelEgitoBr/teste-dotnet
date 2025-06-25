using Questao5.Domain.Entities;

namespace Questao5.Domain.Contracts;

public interface IContaCorrenteRepository
{
    Task<ContaCorrente> ObterContaPorIdAsync(string id);
}
