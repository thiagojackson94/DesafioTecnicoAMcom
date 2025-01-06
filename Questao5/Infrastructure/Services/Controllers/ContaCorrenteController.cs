using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Validation;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediatorHandler;

        public ContaCorrenteController(IMediator mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost]
        [Route("movimentar")]
        [ProducesResponseType(typeof(MovimentarContaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> MovimentarContaCorrenteAsync(MovimentarContaCommand command)
        {
            var result = await _mediatorHandler.Send(command);
            return Ok(result);
        }

        [HttpGet("saldo")]
        [ProducesResponseType(typeof(SaldoContaResponse), 200)]
        public async Task<IActionResult> GetSaldoAsync([FromQuery] string id)
        {
            var response = await _mediatorHandler.Send(new SaldoContaQuery(id));
            return Ok(response);
        }
    }
}
