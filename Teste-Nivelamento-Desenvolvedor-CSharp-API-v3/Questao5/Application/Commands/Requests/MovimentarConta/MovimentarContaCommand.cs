using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests.MovimentarConta;

public class MovimentarContaCommand : IRequest<ResponseCommand<string>>
{
    public string IdRequisicao { get; set; } = string.Empty;
    public string IdContaCorrente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string TipoMovimento { get; set; } = string.Empty;
}

