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
            "SELECT idcontacorrente, numero, nome, ativo FROM contacorrente WHERE idcontacorrente = @id",
            new { id });
    }

    public async Task<decimal> SomarMovimentosAsync(string idConta, string tipo)
    {
        using var conn = new SqliteConnection(_config.Name);
        var result = await conn.ExecuteScalarAsync<decimal?>(
            "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @id AND tipomovimento = @tipo",
            new { id = idConta, tipo });

        return result ?? 0.00m;
    }
}
