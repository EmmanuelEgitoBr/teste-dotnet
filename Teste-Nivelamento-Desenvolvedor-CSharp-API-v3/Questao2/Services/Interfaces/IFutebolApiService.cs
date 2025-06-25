using Questao2.Models;
using Refit;

namespace Questao2.Services.Interfaces;

public interface IFutebolApiService
{
    [Get("/football_matches")]
    Task<PartidaResponse> RetornarPartidasAsync(
    [Query] int year,
    [Query] string? team1 = null,
    [Query] string? team2 = null,
    [Query] int page = 1);
}
