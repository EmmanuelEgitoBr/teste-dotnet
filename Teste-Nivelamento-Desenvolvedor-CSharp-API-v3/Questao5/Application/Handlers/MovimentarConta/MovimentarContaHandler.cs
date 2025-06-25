using MediatR;
using Questao5.Application.Commands.Requests.MovimentarConta;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Contracts;
using Questao5.Domain.Contracts.MovimentarConta;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using System.Text.Json;

namespace Questao5.Application.Handlers.MovimentarConta;

public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand, ResponseCommand<string>>
{
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IContaCorrenteRepository _contaRepository;
    private readonly IIdempotenciaRepository _idempotenciaRepository;

    public MovimentarContaHandler(
        IMovimentoRepository movimentoRepository,
        IContaCorrenteRepository contaRepository,
        IIdempotenciaRepository idempotenciaRepository)
    {
        _movimentoRepository = movimentoRepository;
        _contaRepository = contaRepository;
        _idempotenciaRepository = idempotenciaRepository;
    }

    public async Task<ResponseCommand<string>> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
    {
        var resultadoSalvo = await _idempotenciaRepository.ObterResultadoAsync(request.IdRequisicao);
        if (resultadoSalvo != null)
        {
            var result = JsonSerializer.Deserialize<ResponseCommand<string>>(resultadoSalvo);
            return result!;
        }

        var conta = await _contaRepository.ObterContaPorIdAsync(request.IdContaCorrente);
        if (conta == null)
            return new ResponseCommand<string> {
                Success = false,
                ErrorMessage = "Conta inválida",
                ErrorType = ErroValidacao.INVALID_ACCOUNT.ToString()
            };

        if (!conta.Ativo)
            return new ResponseCommand<string>
            {
                Success = false,
                ErrorMessage = "Conta inativa",
                ErrorType = ErroValidacao.INACTIVE_ACCOUNT.ToString()
            };

        if (request.Valor <= 0)
            return new ResponseCommand<string>
            {
                Success = false,
                ErrorMessage = "Valor deve ser positivo",
                ErrorType = ErroValidacao.INVALID_VALUE.ToString()
            };
        
        if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
            return new ResponseCommand<string>
            {
                Success = false,
                ErrorMessage = "Tipo inválido",
                ErrorType = ErroValidacao.INVALID_TYPE.ToString()
            };

        var movimento = new Movimento
        {
            IdMovimento = request.IdRequisicao,
            IdContaCorrente = request.IdContaCorrente,
            TipoMovimento = request.TipoMovimento,
            Valor = request.Valor,
            DataMovimento = DateTime.UtcNow
        };

        await _movimentoRepository.InserirMovimentoAsync(movimento);

        return new ResponseCommand<string>
        {
            Success = true,
            Data = movimento.IdMovimento
        };
    }
}


