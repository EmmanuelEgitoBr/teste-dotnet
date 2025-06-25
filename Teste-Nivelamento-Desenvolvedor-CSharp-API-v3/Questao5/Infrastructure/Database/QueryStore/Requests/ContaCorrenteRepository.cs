using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Contracts;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Requests;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly DatabaseConfig _config;

    public ContaCorrenteRepository(DatabaseConfig config)
    {
        _config = config;
    }

    public async Task<ContaCorrente> ObterContaPorIdAsync(string id)
    {
        using var connection = new SqliteConnection(_config.Name);
        return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(
            "SELECT idcontacorrente, ativo FROM contacorrente WHERE idcontacorrente = @id",
            new { id });
    }
}
