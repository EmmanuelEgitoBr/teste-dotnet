using Questao2.Models;
using Questao2.Services.Interfaces;

namespace Questao2.Services;

public class ContadorGolsService
{
    private readonly IFutebolApiService _api;

    public ContadorGolsService(IFutebolApiService api)
    {
        _api = api;
    }

    public async Task<int> RetornarTotalGolsPorTimeAsync(string time, int ano)
    {
        int totalGols = 0;

        // Gols como time1
        totalGols += await RetornarNumeroGolsAsync(time, ano, isTime1: true);

        // Gols como time2
        totalGols += await RetornarNumeroGolsAsync(time, ano, isTime1: false);

        return totalGols;
    }

    private async Task<int> RetornarNumeroGolsAsync(string time, int ano, bool isTime1)
    {
        int gols = 0, pagina = 1;
        PartidaResponse response;

        do
        {
            response = await _api.RetornarPartidasAsync(
                year: ano,
                team1: isTime1 ? time : null,
                team2: isTime1 ? null : time,
                page: pagina
            );

            foreach (var partida in response.Partidas)
            {
                var golStr = isTime1 ? partida.GolsTime1 : partida.GolsTime2;
                if (int.TryParse(golStr, out int parsed))
                    gols += parsed;
            }

            pagina++;
        } while (pagina <= response.TotalPaginas);

        return gols;
    }
}
