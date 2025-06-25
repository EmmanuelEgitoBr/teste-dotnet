using MediatR;
using Questao5.Application.Dtos;
using Questao5.Application.Queries.Requests.ConsultaSaldo;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Contracts;
using Questao5.Domain.Contracts.MovimentarConta;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Handlers.ConsultarSaldo;
public class ConsultaSaldoHandler : IRequestHandler<ConsultarSaldoQuery, ResponseQuery<ConsultaSaldoDto>>
{
    private readonly IContaCorrenteRepository _contaRepo;
    public ConsultaSaldoHandler(
    IContaCorrenteRepository contaRepo)
    {
        _contaRepo = contaRepo;
    }

    public async Task<ResponseQuery<ConsultaSaldoDto>> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
    {
        var conta = await _contaRepo.ObterContaPorIdAsync(request.IdContaCorrente);

        if (conta == null)
            return new ResponseQuery<ConsultaSaldoDto>
            {
                Success = false,
                ErrorMessage = "Conta inválida",
                ErrorType = ErroValidacao.INVALID_ACCOUNT.ToString()
            };

        if (!conta.Ativo)
            return new ResponseQuery<ConsultaSaldoDto>
            {
                Success = false,
                ErrorMessage = "Conta inativa",
                ErrorType = ErroValidacao.INACTIVE_ACCOUNT.ToString()
            };

        decimal creditos = await _contaRepo.SomarMovimentosAsync(request.IdContaCorrente, "C");
        decimal debitos = await _contaRepo.SomarMovimentosAsync(request.IdContaCorrente, "D");

        var saldo = creditos - debitos;

        var result = new ConsultaSaldoDto
        {
            Numero = conta.Numero,
            Nome = conta.Nome,
            DataConsulta = DateTime.UtcNow,
            Saldo = saldo
        };

        return new ResponseQuery<ConsultaSaldoDto>
        {
            Success = true,
            Data = result
        };
    }
}
