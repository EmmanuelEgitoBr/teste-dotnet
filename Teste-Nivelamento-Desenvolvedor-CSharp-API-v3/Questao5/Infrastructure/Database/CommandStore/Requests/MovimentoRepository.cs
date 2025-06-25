using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Contracts.MovimentarConta;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig _config;

        public MovimentoRepository(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task InserirMovimentoAsync(Movimento movimento)
        {
            using var connection = new SqliteConnection(_config.Name);
            movimento.IdMovimento = Guid.NewGuid().ToString();
            await connection.ExecuteAsync(@"
            INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                new
                {
                    movimento.IdMovimento,
                    movimento.IdContaCorrente,
                    movimento.DataMovimento,
                    movimento.TipoMovimento,
                    movimento.Valor
                });
        }

        public async Task<bool> MovimentoExisteAsync(string idMovimento)
        {
            using var connection = new SqliteConnection(_config.Name);
            var id = await connection.QueryFirstOrDefaultAsync<string?>(
                "SELECT idmovimento FROM movimento WHERE idmovimento = @id", new { id = idMovimento });
            return !String.IsNullOrEmpty(id);
        }
    }
}
