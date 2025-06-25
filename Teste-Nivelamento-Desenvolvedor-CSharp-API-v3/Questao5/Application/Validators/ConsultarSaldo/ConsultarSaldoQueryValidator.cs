using FluentValidation;
using Questao5.Application.Queries.Requests.ConsultaSaldo;

namespace Questao5.Application.Validators.ConsultarSaldo;

public class ConsultarSaldoQueryValidator : AbstractValidator<ConsultarSaldoQuery>
{
    public ConsultarSaldoQueryValidator()
    {
        RuleFor(x => x.IdContaCorrente)
            .NotEmpty().WithMessage("Id da conta corrente é obrigatório.");
    }
}
