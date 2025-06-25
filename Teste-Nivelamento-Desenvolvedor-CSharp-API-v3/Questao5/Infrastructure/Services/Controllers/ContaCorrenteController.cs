using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests.MovimentarConta;
using Questao5.Application.Queries.Requests.ConsultaSaldo;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/conta-corrente")]
    [ApiController]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator) => _mediator = mediator;

        [HttpPost("movimentar")]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentarContaCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Success)
                return Ok(new { IdMovimento = result.Data });

            return BadRequest(new { Mensagem = result.ErrorMessage, Tipo = result.ErrorType });
        }

        [HttpGet("{id}/saldo")]
        public async Task<IActionResult> ConsultarSaldo(string id)
        {
            var result = await _mediator.Send(new ConsultarSaldoQuery { IdContaCorrente = id });

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(new { Mensagem = result.ErrorMessage, Tipo = result.ErrorType });
        }
    }
}
