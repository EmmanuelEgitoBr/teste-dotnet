using Questao5.Domain.Contracts.MovimentarConta;
using Questao5.Domain.Contracts;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Application.Validators.MovimentarConta;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Questao5.Configurations;

public static class IoCConfig
{
    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
        services.AddScoped<IMovimentoRepository, MovimentoRepository>();
        services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();

        return services;
    }

    public static IServiceCollection AddApiValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<MovimentarContaCommandValidator>();
        services.AddFluentValidation();
        
        return services;
    }

}
