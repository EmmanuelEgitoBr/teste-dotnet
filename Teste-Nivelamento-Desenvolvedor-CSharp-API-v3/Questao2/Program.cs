using Microsoft.Extensions.DependencyInjection;
using Questao2.Services;
using Questao2.Services.Interfaces;
using Refit;

var services = new ServiceCollection();

services.AddRefitClient<IFutebolApiService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://jsonmock.hackerrank.com/api");
    });

services.AddTransient<ContadorGolsService>();

var provider = services.BuildServiceProvider();
var goalService = provider.GetRequiredService<ContadorGolsService>();

// Execução principal
var team1 = "Paris Saint-Germain";
int year1 = 2013;
int goals1 = await goalService.RetornarTotalGolsPorTimeAsync(team1, year1);
Console.WriteLine($"Team {team1} scored {goals1} goals in {year1}");

var team2 = "Chelsea";
int year2 = 2014;
int goals2 = await goalService.RetornarTotalGolsPorTimeAsync(team2, year2);
Console.WriteLine($"Team {team2} scored {goals2} goals in {year2}");
