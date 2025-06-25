using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests.MovimentarConta;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentarContaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentarContaController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentarContaCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Success)
                return Ok(new { IdMovimento = result.Data });

            return BadRequest(new { Mensagem = result.ErrorMessage, Tipo = result.ErrorType });
        }
    }
}
