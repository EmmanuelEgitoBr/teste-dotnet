using NSubstitute;
using Questao5.Application.Commands.Requests.MovimentarConta;
using Questao5.Application.Handlers.MovimentarConta;
using Questao5.Domain.Contracts.MovimentarConta;
using Questao5.Domain.Contracts;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Xunit;
using FluentAssertions;
using Questao5.Application.Commands.Responses;
using System.Text.Json;

namespace Questao5.UnitTests.MovimentarConta;

public class MovimentarContaHandlerTests
{
    private readonly IMovimentoRepository _movimentoRepo = Substitute.For<IMovimentoRepository>();
    private readonly IContaCorrenteRepository _contaRepo = Substitute.For<IContaCorrenteRepository>();
    private readonly IIdempotenciaRepository _idempotenciaRepo = Substitute.For<IIdempotenciaRepository>();

    private readonly MovimentarContaHandler _handler;

    public MovimentarContaHandlerTests()
    {
        _handler = new MovimentarContaHandler(_movimentoRepo, _contaRepo, _idempotenciaRepo);
    }

    [Fact]
    public async Task Deve_Retornar_IdMovimento_Quando_Sucesso()
    {
        // Arrange
        var requisicao = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = Guid.NewGuid().ToString(),
            TipoMovimento = "C",
            Valor = 100
        };

        _idempotenciaRepo.ObterResultadoAsync(requisicao.IdRequisicao).Returns((string?)null);
        _contaRepo.ObterContaPorIdAsync(requisicao.IdContaCorrente).Returns(new ContaCorrente 
        { IdContaCorrente = requisicao.IdContaCorrente, Ativo = true });

        // Act
        var resultado = await _handler.Handle(requisicao, CancellationToken.None);

        // Assert
        resultado.Success.Should().BeTrue();
        resultado.Data.Should().Be(requisicao.IdRequisicao);

        await _movimentoRepo.Received(1).InserirMovimentoAsync(Arg.Is<Movimento>(m =>
            m.IdMovimento == requisicao.IdRequisicao &&
            m.Valor == requisicao.Valor &&
            m.TipoMovimento == requisicao.TipoMovimento
        ));
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Se_Conta_Nao_Existe()
    {
        var command = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = Guid.NewGuid().ToString(),
            TipoMovimento = "D",
            Valor = 50
        };

        _idempotenciaRepo.ObterResultadoAsync(command.IdRequisicao).Returns((string?)null);
        _contaRepo.ObterContaPorIdAsync(command.IdContaCorrente)!.Returns((ContaCorrente?)null);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Success.Should().BeFalse();
        resultado.ErrorType.Should().Be(ErroValidacao.INVALID_ACCOUNT.ToString());
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Se_Conta_Inativa()
    {
        var command = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = Guid.NewGuid().ToString(),
            TipoMovimento = "C",
            Valor = 75
        };

        _idempotenciaRepo.ObterResultadoAsync(command.IdRequisicao).Returns((string?)null);
        _contaRepo.ObterContaPorIdAsync(command.IdContaCorrente).Returns(new ContaCorrente 
        { IdContaCorrente = command.IdContaCorrente, Ativo = false });

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Success.Should().BeFalse();
        resultado.ErrorType.Should().Be(ErroValidacao.INACTIVE_ACCOUNT.ToString());
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Se_Idempotencia_Existe()
    {
        var command = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = Guid.NewGuid().ToString(),
            TipoMovimento = "D",
            Valor = 200
        };

        var resultadoSerializado = JsonSerializer.Serialize(
            new ResponseCommand<string> {
                Success = true,
                Data = command.IdRequisicao
            });
        _idempotenciaRepo.ObterResultadoAsync(command.IdRequisicao).Returns(resultadoSerializado);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Success.Should().BeTrue();
        resultado.Data.Should().Be(command.IdRequisicao);
        await _contaRepo.DidNotReceive().ObterContaPorIdAsync(Arg.Any<string>());
    }
}
