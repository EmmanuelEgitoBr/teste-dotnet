using MediatR;
using Questao5.Application.Dtos;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests.ConsultaSaldo;

public class ConsultarSaldoQuery : IRequest<ResponseQuery<ConsultaSaldoDto>>
{
    public string IdContaCorrente { get; set; } = string.Empty;
}
