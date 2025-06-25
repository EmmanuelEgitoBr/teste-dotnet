using FluentValidation;
using Questao5.Application.Commands.Requests.MovimentarConta;

namespace Questao5.Application.Validators.MovimentarConta;

public class MovimentarContaCommandValidator : AbstractValidator<MovimentarContaCommand>
{
    public MovimentarContaCommandValidator()
    {
        RuleFor(x => x.IdContaCorrente)
            .NotEmpty().WithMessage("Id da conta corrente é obrigatório.");
    }
}
