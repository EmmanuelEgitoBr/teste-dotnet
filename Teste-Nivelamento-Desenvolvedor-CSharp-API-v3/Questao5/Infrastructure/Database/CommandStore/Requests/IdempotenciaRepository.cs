using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Contracts;
using Questao5.Infrastructure.Sqlite;
using System.Text.Json;

namespace Questao5.Infrastructure.Database.CommandStore.Requests;

public class IdempotenciaRepository : IIdempotenciaRepository
{
    private readonly DatabaseConfig _config;

    public IdempotenciaRepository(DatabaseConfig config)
    {
        _config = config;
    }

    public async Task<string?> ObterResultadoAsync(string chave)
    {
        using var conn = new SqliteConnection(_config.Name);
        return await conn.QueryFirstOrDefaultAsync<string>(
            "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @chave", new { chave = chave.ToString() });
    }

    public async Task RegistrarAsync(string chave, object requisicao, object resultado)
    {
        using var conn = new SqliteConnection(_config.Name);
        await conn.ExecuteAsync(@"
            INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
            VALUES (@chave, @req, @res)",
            new
            {
                chave = chave.ToString(),
                req = JsonSerializer.Serialize(requisicao),
                res = JsonSerializer.Serialize(resultado)
            });
    }
}

