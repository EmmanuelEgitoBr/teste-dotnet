using System.Text.Json.Serialization;

namespace Questao2.Models;

public class Partida
{
    [JsonPropertyName("competition")]
    public string Competicao { get; set; } = string.Empty;

    [JsonPropertyName("year")]
    public int Ano { get; set; }

    [JsonPropertyName("round")]
    public string Rodada { get; set; } = string.Empty;

    [JsonPropertyName("team1")]
    public string Time1 { get; set; } = string.Empty;

    [JsonPropertyName("team2")]
    public string Time2 { get; set; } = string.Empty;

    [JsonPropertyName("team1goals")]
    public string GolsTime1 { get; set; } = string.Empty;

    [JsonPropertyName("team2goals")]
    public string GolsTime2 { get; set; } = string.Empty;
}
