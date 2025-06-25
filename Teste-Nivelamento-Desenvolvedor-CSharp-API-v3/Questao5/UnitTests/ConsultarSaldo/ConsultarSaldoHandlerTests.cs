using NSubstitute;
using Questao5.Application.Handlers.ConsultarSaldo;
using Questao5.Application.Queries.Requests.ConsultaSaldo;
using Questao5.Domain.Contracts.MovimentarConta;
using Questao5.Domain.Contracts;
using Questao5.Domain.Enumerators;
using Xunit;
using Questao5.Domain.Entities;
using FluentAssertions;

namespace Questao5.UnitTests.ConsultarSaldo;

public class ConsultarSaldoHandlerTests
{
    private readonly IContaCorrenteRepository _contaRepo = Substitute.For<IContaCorrenteRepository>();
    
    private readonly ConsultarSaldoHandler _handler;

    public ConsultarSaldoHandlerTests()
    {
        _handler = new ConsultarSaldoHandler(_contaRepo);
    }

    [Fact]
    public async Task Deve_Retornar_Saldo_Calculado_Corretamente()
    {
        // Arrange
        var idConta = Guid.NewGuid().ToString();
        _contaRepo.ObterContaPorIdAsync(idConta).Returns(new ContaCorrente 
        { 
            IdContaCorrente = idConta, 
            Numero = 123, 
            Nome = "Cliente Teste", 
            Ativo = true 
        });
        _contaRepo.SomarMovimentosAsync(idConta, "C").Returns(500.00m);
        _contaRepo.SomarMovimentosAsync(idConta, "D").Returns(130.00m);

        var query = new ConsultarSaldoQuery { IdContaCorrente = idConta };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data!.Saldo.Should().Be(370.00m);
        result.Data.Nome.Should().Be("Cliente Teste");
        result.Data.Numero.Should().Be(123);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Conta_Nao_Existe()
    {
        var idConta = Guid.NewGuid().ToString();
        _contaRepo.ObterContaPorIdAsync(idConta)!.Returns((ContaCorrente?)null);

        var query = new ConsultarSaldoQuery { IdContaCorrente = idConta };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorType.Should().Be(ErroValidacao.INVALID_ACCOUNT.ToString());
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Conta_Inativa()
    {
        var idConta = Guid.NewGuid().ToString();
        _contaRepo.ObterContaPorIdAsync(idConta).Returns(new ContaCorrente 
        { 
            IdContaCorrente = idConta, 
            Numero = 123, 
            Nome = "Inativo", 
            Ativo = false 
        });

        var query = new ConsultarSaldoQuery { IdContaCorrente = idConta };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorType.Should().Be(ErroValidacao.INACTIVE_ACCOUNT.ToString());
    }

    [Fact]
    public async Task Deve_Retornar_Saldo_Zero_Se_Sem_Movimentacoes()
    {
        var idConta = Guid.NewGuid().ToString();
        _contaRepo.ObterContaPorIdAsync(idConta).Returns(new ContaCorrente 
        { 
            IdContaCorrente = idConta, 
            Numero = 111, 
            Nome = "Sem Movimentos", 
            Ativo = true 
        });

        _contaRepo.SomarMovimentosAsync(idConta, "C").Returns(0.00m);
        _contaRepo.SomarMovimentosAsync(idConta, "D").Returns(0.00m);

        var query = new ConsultarSaldoQuery { IdContaCorrente = idConta };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.Saldo.Should().Be(0.00m);
    }
}
