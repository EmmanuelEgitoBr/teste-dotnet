using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Questao2.Models;

public class PartidaResponse
{
    [JsonPropertyName("page")]
    public int PaginaAtual { get; set; }

    [JsonPropertyName("per_page")]
    public int PorPagina { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPaginas { get; set; }

    [JsonPropertyName("data")]
    public List<Partida> Partidas { get; set; } = new List<Partida>();
}
